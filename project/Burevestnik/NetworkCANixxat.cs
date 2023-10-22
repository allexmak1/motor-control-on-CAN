using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.ComponentModel;
using Ixxat.Vci3;
using Ixxat.Vci3.Bal;
using Ixxat.Vci3.Bal.Can;

namespace Burevestnik {
    public class NetworkCANixxat {

        private static CanClient _canClient = new CanClient();

        static NetworkCANixxat() {
            Application.ApplicationExit += delegate {
                if (_canClient != null) _canClient.Dispose();
            };
        }

        /// <summary>Автоматическая работа CAN: определяет битрейт сети</summary>
        /// <param name="detectBitrates"></param>
        public static void ConfigDetect(params int[] detectBitrates) {
            _canClient.ConfigDetect(detectBitrates);
        }

        /// <summary>Автоматическая работа CAN: задаёт битрейт</summary>
        /// <param name="portBitrates">Общий битрейт для всех портов, или, если несколько аргументов, уникальный битрейт для каждого порта</param>
        public static void ConfigPorts(params int[] portBitrates) {
            _canClient.ConfigPorts(portBitrates);
        }


        // ОТПРАВКА СООБЩЕНИЙ

        public static void Send(long deviceId, byte portNumber, Burevestnik.CanMessage message) {
            if (_canClient._devices.ContainsKey(deviceId)) {
                _canClient._devices[deviceId].Send(portNumber, message);
            }
        }

        public static void Send(byte portNumber, Burevestnik.CanMessage message) {
            foreach (var device in _canClient._devices.Values) {
                device.Send(portNumber, message);
            }
        }

        public static void Send(Burevestnik.CanMessage message) {
            foreach (var device in _canClient._devices.Values) {
                device.Send(message);
            }
        }


        // ПРИЁМ СООБЩЕНИЙ

        public delegate void CanMessageReceiveHandler(CanSocket socket, Burevestnik.CanMessage message);
        /// <summary>Происходит при получении CAN сообщения.</summary>
        public static event CanMessageReceiveHandler MessageReceived;


        // ДОСТУП К УСТРОЙСТВАМ

        /// <summary>Возвращает true, если хотябы одно устройство USB-to-CAN подключено.</summary>
        public static bool IsAnyDeviceConnected { get {
            bool anyDeviceConnected = false;
            foreach (var device in _canClient._devices.Values) {
                if (device.Connected) {
                    anyDeviceConnected = true;
                    break;
                }
            }
            return anyDeviceConnected;
        } }

        public static CanSocket LastActiveSocket { get {
            if (_lastActiveSocket == null && Sockets.Count > 0) {
                _lastActiveSocket = Sockets[0];
            }
            return _lastActiveSocket;
        } }
        private static CanSocket _lastActiveSocket = null;

        public static List<CanSocket> Sockets {
            get { return _canClient._sockets; }
        }

        public static event EventHandler SocketsChanged;

        // ВСТРОЕННЫЕ КЛАССЫ

        public class CanClient : IDisposable {

            private IVciDeviceList _ixxatDeviceList;
            private AutoResetEvent _ixxatDeviceListChangeEvent = new AutoResetEvent(false);
            private Thread _ixxatDeviceListChangeThread;
            private int _ixxatDeviceListChangeThreadQuitFlag = 0;

            private System.Timers.Timer _statusTimer = new System.Timers.Timer();

            internal Dictionary<long, CanDevice> _devices = new Dictionary<long, CanDevice>();
            internal List<CanSocket> _sockets = new List<CanSocket>();

            private CanBitrate[] _bitratesForDetection = null;
            private Dictionary<byte, CanBitrate> _exactBitrates = new Dictionary<byte, CanBitrate>();

            private int _socketStatesSinked = -1;

            public CanClient() {
                IVciDeviceManager ixxatDeviceManager = VciServer.GetDeviceManager();
                _ixxatDeviceList = ixxatDeviceManager.GetDeviceList();
                ixxatDeviceManager.Dispose();

                _statusTimer.Interval = 100;
                _statusTimer.Elapsed += delegate {
                    var anyStateUpdated = false;

                    foreach (var socket in _sockets) {
                        if (socket.UpdateState()) {
                            anyStateUpdated = true;
                        }
                    }

                    if (anyStateUpdated || _socketStatesSinked != 0) {
                        var invoked = Core.RaiseEvent(SocketsChanged, new object[] { this, EventArgs.Empty });
                        if (_socketStatesSinked != 0) {
                            _socketStatesSinked = invoked;
                        }
                    }
                };

                _ixxatDeviceList.AssignEvent(_ixxatDeviceListChangeEvent);
                _ixxatDeviceListChangeThread = new Thread(new ThreadStart(delegate {
                    while (true) {
                        _ixxatDeviceListChangeEvent.WaitOne();
                        if (_ixxatDeviceListChangeThreadQuitFlag != 0) break;

                        UpdateDevices();
                    }
                }));

                _ixxatDeviceListChangeThread.Start();
                _ixxatDeviceListChangeEvent.Set();

                _statusTimer.Start();
            }

            private void UpdateDevices() {
                var ixxatDevicesCount = 0;

                foreach (IVciDevice ixxatDevice in _ixxatDeviceList) {
                    ixxatDevicesCount += 1;

                    var ixxatDeviceId = GetId(ixxatDevice.UniqueHardwareId);
                    if (!_devices.ContainsKey(ixxatDeviceId)) {
                        var newDevice = new CanDevice(ixxatDevice);
                        _devices.Add(ixxatDeviceId, newDevice);
                        foreach (var socket in newDevice.Sockets.Values) _sockets.Add(socket);
                    }

                    ixxatDevice.Dispose();
                }

                foreach (var id in _devices.Keys) {
                    bool ixxatConnected = false;

                    foreach (IVciDevice ixxatDevice in _ixxatDeviceList) {
                        if (GetId(ixxatDevice.UniqueHardwareId) == id) {
                            _devices[id].Init(ixxatDevice);
                            ixxatConnected = true;
                            ixxatDevice.Dispose();
                            break;
                        }
                        ixxatDevice.Dispose();
                    }

                    if (!ixxatConnected) {
                        _devices[id].Deinit();
                    }
                }

                Config();
                Core.RaiseEvent(SocketsChanged, new object[] { this, EventArgs.Empty });
            }

            public void ConfigPorts(params int[] portBitrates) {
                if (portBitrates.Length > 1) {
                    if (_exactBitrates.ContainsKey(0xFF)) _exactBitrates.Remove(0xFF);
                    for (byte i = 0; i < portBitrates.Length; i++) {
                        _exactBitrates[i] = GetCanBitrate(portBitrates[i]);
                    }
                } else {
                    _exactBitrates.Clear();
                    if (portBitrates.Length == 1) {
                        _exactBitrates[0xFF] = GetCanBitrate(portBitrates[0]);
                    }
                }
            }

            public void ConfigDetect(params int[] bitratesForDetection) {
                _bitratesForDetection = GetCanBitrates(bitratesForDetection);
            }

            public void Config() {
                for (byte i = 0; i < _sockets.Count; i++) {
                    if (_bitratesForDetection != null && _bitratesForDetection.Length > 0) {
                        _sockets[i].BitratesForDetection = _bitratesForDetection;
                    } else if (_exactBitrates.Count > 0) {
                        if (_exactBitrates.ContainsKey(0xFF)) {
                            _sockets[i].BitrateExact = _exactBitrates[0xFF];
                        } else if (_exactBitrates.Count >= i + 1) {
                            _sockets[i].BitrateExact = _exactBitrates[i];
                        }
                    }
                    _sockets[i].Config();
                }
                
            }

            public void Dispose() {
                foreach (CanDevice canDevice in _devices.Values) canDevice.Dispose();

                Interlocked.Exchange(ref _ixxatDeviceListChangeThreadQuitFlag, 1);
                _ixxatDeviceListChangeEvent.Set();
                _ixxatDeviceList.Dispose();
            }
        }

        public class CanDevice : IDisposable {
            public string SerialNumber { get { return _serialNumber; } }
            private string _serialNumber;
            public long Id { get { return _id; } }
            private long _id;

            internal Dictionary<byte, CanSocket> Sockets = new Dictionary<byte, CanSocket>();

            private IBalObject _ixxatBalObject = null;

            public bool Connected { get { return _ixxatBalObject != null; } }

            public CanDevice(IVciDevice device) {
                _id = GetId(device.UniqueHardwareId);
                _serialNumber = GetSerialNumber(device.UniqueHardwareId);
                _ixxatBalObject = device.OpenBusAccessLayer();

                foreach (IBalResource resourse in _ixxatBalObject.Resources) {
                    if (resourse.BusType == VciBusType.Can) {
                        var socket = new CanSocket(this, resourse.BusPort);
                        Sockets.Add((byte)(resourse.BusPort + 1), socket);
                        socket.Init(_ixxatBalObject);
                    }
                    resourse.Dispose();
                }
            }

            public void Init(IVciDevice device) {
                if (_ixxatBalObject != null) return;

                _ixxatBalObject = device.OpenBusAccessLayer();
                foreach (IBalResource resourse in _ixxatBalObject.Resources) {
                    if (resourse.BusType == VciBusType.Can) {
                        byte busPort = (byte)(resourse.BusPort + 1);
                        if (Sockets.ContainsKey(busPort)) {
                            Sockets[busPort].Init(_ixxatBalObject);
                        }
                    }
                    resourse.Dispose();
                }
            }

            public void Deinit() {
                if (_ixxatBalObject != null) _ixxatBalObject.Dispose();
                _ixxatBalObject = null;
                foreach (var socket in Sockets.Values) socket.Deinit();
            }

            public void Send(byte portNumber, Burevestnik.CanMessage message) {
                if (Sockets.ContainsKey(portNumber)) {
                    Sockets[portNumber].Send(message);
                }
            }

            public void Send(Burevestnik.CanMessage message) {
                foreach (CanSocket canSocket in Sockets.Values) canSocket.Send(message);
            }

            public void Dispose() {
                foreach (CanSocket canSocket in Sockets.Values) canSocket.Dispose();
                if (_ixxatBalObject != null) _ixxatBalObject.Dispose();
            }
        }

        public class CanSocket : IDisposable {
            public bool Controllable { get {
                return _ixxatCanControl != null;
            } }

            public string Name { get { return String.Format("{0} CAN-{1}", _parentDevice.SerialNumber, _busPort + 1); } }
            public int Port { get { return _busPort + 1; } }

            public string StateTextSimple { get {
                switch (_state) {
                    case CanSocketState.NoDevice: return "нет IXXAT";
                    case CanSocketState.Stopped: return "остановлен";
                    case CanSocketState.BusOff:
                    case CanSocketState.TXPending: return "ошибки";
                    case CanSocketState.WorkingNoMessages:
                    case CanSocketState.Working: return "";
                    default: return "-";
                }
            } }

            public string StateText { get {
                switch (_state) {
                    case CanSocketState.NoDevice: return "Нет IXXAT";
                    case CanSocketState.Stopped: return "Остановлен";
                    case CanSocketState.BusOff: return "BusOff";
                    case CanSocketState.TXPending: return "TXPending";
                    case CanSocketState.WorkingNoMessages: return "Нет сообщений";
                    case CanSocketState.Working: return "";
                    default: return "-";
                }
            } }
            public CanSocketState State { get { return _state; } }
            private CanSocketState _state = CanSocketState.Undefined;

            /// <summary>Возвращает скорость в KBit. 0 — скорость не заданна или порт остановлен.</summary>
            public int Bitrate { get {
                if (_ixxatCanChannel == null || _ixxatCanMessageReaderThreadQuitFlag != 0) {
                    return 0;
                } else {
                    switch (_ixxatCanChannel.LineStatus.Bitrate.AsInt16) {
                        case 0x1C31: return 10;
                        case 0x1C18: return 20;
                        case 0x1C09: return 50;
                        case 0x1C04: return 100;
                        case 0x1C03: return 125;
                        case 0x1C01: return 250;
                        case 0x1C00: return 500;
                        case 0x1600: return 800;
                        case 0x1400: return 1000;
                        default: return _ixxatCanChannel.LineStatus.Bitrate.AsInt16;
                    }
                }
            } }

            public bool StateTransmitPending { get { return _ixxatCanChannel != null && _ixxatCanChannel.LineStatus.IsTransmitPending; } }
            public bool StateDataOverrun { get { return _ixxatCanChannel != null && _ixxatCanChannel.LineStatus.HasDataOverrun; } }
            public bool StateErrorWarningLevel { get { return _ixxatCanChannel != null && _ixxatCanChannel.LineStatus.HasErrorOverrun; } }
            public bool StateBusOff { get { return _ixxatCanChannel != null && _ixxatCanChannel.LineStatus.IsBusOff; } }


            private CanDevice _parentDevice;
            private byte _busPort;

            private IBalObject _ixxatBalObject;

            private ICanChannel _ixxatCanChannel = null;
            private ICanControl _ixxatCanControl = null;

            public CanBitrate BitrateExact = CanBitrate.Empty;
            public CanBitrate[] BitratesForDetection = null;

            private ICanMessageReader _ixxatCanMessageReader;
            private AutoResetEvent _ixxatCanMessageReaderEvent = new AutoResetEvent(false);
            private Thread _ixxatCanMessageReaderThread;
            private int _ixxatCanMessageReaderThreadQuitFlag = 0;

            private ICanMessageWriter _ixxatCanMessageWriter;

            private int _noMessagesCounter = 0;
            private int _errorsCounter = 0;

            public CanSocket(CanDevice parentDevice, byte busPort) {
                _parentDevice = parentDevice;
                _busPort = busPort;

                _ixxatCanMessageReaderThread = new Thread(new ThreadStart(delegate {
                    while (true) {
                        _ixxatCanMessageReaderEvent.WaitOne();
                        if (_ixxatCanMessageReaderThreadQuitFlag != 0) break;
                        if (_ixxatCanChannel == null) continue;

                        var messages = new Ixxat.Vci3.Bal.Can.CanMessage[_ixxatCanMessageReader.FillCount];
                        _ixxatCanMessageReader.ReadMessages(messages);

                        foreach (var ixxatMessage in messages) {
                            switch (ixxatMessage.FrameType) {
                                case CanMsgFrameType.Data:
                                    if (ixxatMessage.SelfReceptionRequest) return;

                                    Interlocked.Exchange(ref _noMessagesCounter, 0);
                                    if (_lastActiveSocket != this) { _lastActiveSocket = this; }

                                    Core.RaiseEvent(MessageReceived, new object[] {
                                        this,
                                        new Burevestnik.CanMessage(ixxatMessage)
                                    });
                                    break;
                            }
                        }

                        // testing
                        //Core.RaiseEvent(MessageReceived, new object[] {
                        //    _parentDevice.Id,
                        //    (byte)(_busPort + 1),
                        //    new byte[12]
                        //});
                    }
                }));

                _ixxatCanMessageReaderThread.Start();
            }

            public void Init(IBalObject ixxatBalObject) {
                if (_ixxatBalObject != null) _ixxatBalObject.Dispose();
                if (_ixxatCanChannel != null) _ixxatCanChannel.Dispose();
                if (_ixxatCanMessageReader != null) _ixxatCanMessageReader.Dispose();
                if (_ixxatCanMessageWriter != null) _ixxatCanMessageWriter.Dispose();
                if (_ixxatCanControl != null) _ixxatCanControl.Dispose();

                _ixxatBalObject = ixxatBalObject;

                _ixxatCanChannel = _ixxatBalObject.OpenSocket(_busPort, typeof(ICanChannel)) as ICanChannel;
                _ixxatCanChannel.Initialize(256, 128, false);
                _ixxatCanChannel.Activate();

                _ixxatCanMessageReader = _ixxatCanChannel.GetMessageReader();
                _ixxatCanMessageReader.Threshold = 1;
                _ixxatCanMessageReader.AssignEvent(_ixxatCanMessageReaderEvent);
                
                _ixxatCanMessageWriter = _ixxatCanChannel.GetMessageWriter();

                if (TryObtainControl()) {
                    Config();
                }
                UpdateState();
            }

            public void Deinit() {
                if (_ixxatCanChannel != null) _ixxatCanChannel.Dispose();
                if (_ixxatCanMessageReader != null) _ixxatCanMessageReader.Dispose();
                if (_ixxatCanMessageWriter != null) _ixxatCanMessageWriter.Dispose();
                if (_ixxatCanControl != null) _ixxatCanControl.Dispose();

                _ixxatCanChannel = null;
                _ixxatCanMessageReader = null;
                _ixxatCanMessageWriter = null;
                _ixxatCanControl = null;

                UpdateState();
            }

            public bool TryObtainControl() {
                try {
                    if (_ixxatCanChannel == null) return false;
                    if (_ixxatCanControl != null) return true;
                    _ixxatCanControl = _ixxatBalObject.OpenSocket(_ixxatCanChannel.BusPort, typeof(ICanControl)) as ICanControl;
                    return true;
                } catch (VciException) {
                    _ixxatCanControl = null;
                    return false;
                }
            }

            public void ReleaseControl() {
                if (Controllable) {
                    _ixxatCanControl.Dispose();
                    _ixxatCanControl = null;
                }
            }

            public void Config(int bitrate) {
                BitratesForDetection = null;
                BitrateExact = GetCanBitrate(bitrate);
                if (TryObtainControl()) {
                    Config();
                }
            }

            public void Config(int[] bitrates) {
                BitrateExact = CanBitrate.Empty;
                BitratesForDetection = GetCanBitrates(bitrates);
                if (TryObtainControl()) {
                    Config();
                }
            }

            public void Config() {
                if (BitrateExact != CanBitrate.Empty) {
                    SetBitrate(BitrateExact);
                    Start();
                } else if (BitratesForDetection != null && BitratesForDetection.Length > 0) {
                    SetBitrate(BitratesForDetection);
                    Start();
                }
                UpdateState();
            }

            private void SetBitrate(CanBitrate bitrate) {
                if (!Controllable) TryObtainControl();
                if (Controllable) {
                    _ixxatCanControl.InitLine(
                        CanOperatingModes.Standard | CanOperatingModes.Extended | CanOperatingModes.ErrFrame,
                        BitrateExact);
                    _ixxatCanControl.SetAccFilter(CanFilter.Ext, (uint)CanAccCode.All, (uint)CanAccMask.All);
                }
            }

            private void SetBitrate(CanBitrate[] bitrates) {
                if (!Controllable) TryObtainControl();
                if (Controllable) {
                    int index = _ixxatCanControl.DetectBaud(550, BitratesForDetection);
                    if (index >= 0) {
                        SetBitrate(BitratesForDetection[index]);
                    }
                }
            }

            public void Start() {
                if (!Controllable) return;
                _ixxatCanControl.StartLine();
            }

            public void Stop() {
                if (Controllable) _ixxatCanControl.StopLine();
            }

            public bool UpdateState() {
                var oldState = _state;

                _noMessagesCounter = _noMessagesCounter >= 20 ? 20 : _noMessagesCounter + 1;

                if (_ixxatCanChannel == null || _ixxatCanMessageReaderThreadQuitFlag != 0) {
                    _state = CanSocketState.NoDevice;
                    _errorsCounter = 0;
                } else if (_ixxatCanChannel.LineStatus.IsInInitMode) {
                    _state = CanSocketState.Stopped;
                    _errorsCounter = 0;
                } else if (_ixxatCanChannel.LineStatus.IsBusOff) {
                    _errorsCounter = _errorsCounter < 5 ? _errorsCounter + 1 : 5;
                    if (_errorsCounter == 5) {
                        _state = CanSocketState.BusOff;
                    }
                } else if (_ixxatCanChannel.LineStatus.IsTransmitPending) {
                    _errorsCounter = _errorsCounter < 5 ? _errorsCounter + 1 : 5;
                    if (_errorsCounter == 5) {
                        _state = CanSocketState.TXPending;
                    }
                } else if (_noMessagesCounter == 20) {
                    _state = CanSocketState.WorkingNoMessages;
                    _errorsCounter = 0;
                } else {
                    _state = CanSocketState.Working;
                    _errorsCounter = 0;
                }

                return oldState != _state;
            }

            public void Send(Burevestnik.CanMessage message) {
                // TODO: check can status, maybe restart can
                if (_ixxatCanMessageWriter == null) return;
                _ixxatCanMessageWriter.SendMessage(message.IxxatMessage);
            }

            public void Dispose() {
                Interlocked.Exchange(ref _ixxatCanMessageReaderThreadQuitFlag, 1);
                _ixxatCanMessageReaderEvent.Set();

                if (_ixxatCanChannel != null) _ixxatCanChannel.Dispose();
                if (_ixxatCanMessageReader != null) _ixxatCanMessageReader.Dispose();
                if (_ixxatCanMessageWriter != null) _ixxatCanMessageWriter.Dispose();
                if (_ixxatCanControl != null) _ixxatCanControl.Dispose();

                _ixxatCanChannel = null;
                _ixxatCanMessageReader = null;
                _ixxatCanMessageWriter = null;
                _ixxatCanControl = null;

            }
        }

        public enum CanSocketState {
            NoDevice, Undefined, Stopped, TXPending, BusOff, Working, WorkingNoMessages
        }

        public static CanBitrate GetCanBitrate(int bitrate) {
            switch (bitrate) {
                case 10: return CanBitrate.Cia10KBit;
                case 20: return CanBitrate.Cia20KBit;
                case 50: return CanBitrate.Cia50KBit;
                case 125: return CanBitrate.Cia125KBit;
                case 250: return CanBitrate.Cia250KBit;
                case 500: return CanBitrate.Cia500KBit;
                case 800: return CanBitrate.Cia800KBit;
                case 1000: return CanBitrate.Cia1000KBit;
                default: return CanBitrate.Empty;
            }
        }

        public static CanBitrate[] GetCanBitrates(int[] bitrates) {
            var canBitrates = new List<CanBitrate>();
            foreach (var bitrate in bitrates) {
                var brt = GetCanBitrate(bitrate);
                if (brt != CanBitrate.Empty) canBitrates.Add(brt);
            }

            return canBitrates.ToArray();
        }

        public static string GetSerialNumber(object hardwareId) {
            string serialNumber = "";

            if (hardwareId.GetType() == typeof(Guid)) {
                byte[] tempArray = ((Guid)hardwareId).ToByteArray();
                for (int i = 0; i < tempArray.Length; i++) {
                    if (tempArray[i] == 0) break;
                    serialNumber += (char)tempArray[i];
                }
            } else {
                string tempString = (string)hardwareId;
                for (int i = 0; i < tempString.Length; i++) {
                    if (tempString[i] == 0) break;
                    serialNumber += tempString[i];
                }
            }

            return serialNumber;
        }

        public static long GetId(object hardwareId) {
            ulong id = 0;
            var serialNumber = GetSerialNumber(hardwareId);

            for (int i = 0; i < serialNumber.Length && i < 8; i++) {
                id |= (ulong)serialNumber[i] << i;
            }

            return (long)id;
        }
    }
}
