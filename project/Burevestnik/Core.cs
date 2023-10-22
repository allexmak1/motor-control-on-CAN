using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace Burevestnik {

    public static partial class Core {

        #region Переодическое выполнение задач и таймауты

        /// <summary>Запускает периодическое выполнение функции.</summary>
        /// <param name="interval">Интервал, мс</param>
        /// <param name="handler">Функция, которая будет вызываться с заданным интервалом</param>
        public static void PeriodicTaskStart(int interval, EventHandler handler) {
            var task = new PeriodicTask(interval);
            task.Tick += handler;
            PeriodicTasks.Add(task);
        }
        private static List<PeriodicTask> PeriodicTasks = new List<PeriodicTask>();


        /// <summary>Сообщает, что пришло новое сообщение.</summary>
        /// <param name="id">Идентификатор или дескриптор</param>
        public static void TimeoutCheck(int id) {
            if (TimeoutTasks.ContainsKey(id)) TimeoutTasks[id].Unset();
        }

        /// <summary>Проверяет, что сообщение с заданным id обрабатывается первый раз.</summary>
        /// <param name="id">Идентификатор или дескриптор</param>
        /// <returns>true, если первый раз</returns>
        public static bool TimeoutFirstHandle(int id) {
            return TimeoutTasks.ContainsKey(id) ? TimeoutTasks[id].FirstHandle : false;
        }

        /// <summary>Проверяет, что таймаут с заданным id вышел.</summary>
        /// <param name="id">Идентификатор или дескриптор</param>
        /// <returns>true, если вышел таймаут</returns>
        public static bool TimeoutState(int id) {
            return TimeoutTasks.ContainsKey(id) ? TimeoutTasks[id].TimedOut : false;
        }

        /// <summary>
        /// Запускает проверку наличия периодических сообщений.
        /// <para>Дополнительно нужно вызвать TimeoutCheck(int id) когда приняты сообщения.</para>
        /// </summary>
        /// <param name="interval">Интервал, мс</param>
        /// <param name="id">Идентификатор или дескриптор</param>
        /// <param name="handler">Обработчик <code>void (bool timedOut)</code>, вызывается когда вышел таймаут или пришло сообщение.</param>
        public static void TimeoutStart(int interval, int id, TimeoutTask.TimeoutHandler handler) {
            var task = new TimeoutTask(interval);
            task.TimeoutChanged += handler;
            TimeoutTasks.Add(id, task);
        }
        private static Dictionary<int, TimeoutTask> TimeoutTasks = new Dictionary<int, TimeoutTask>();

        #endregion

        #region Вспомогательные функции

        private static int _applicationQuitFlag = 0;
        static Core() {
            System.Windows.Forms.Application.ApplicationExit += delegate {
                Interlocked.Exchange(ref _applicationQuitFlag, 1);
            };
        }

        public static int RaiseEvent(MulticastDelegate eventDelegate, object[] args) {
            if (eventDelegate == null) return -1;

            var invocations = 0;
            var invoked = 0;

            foreach (var delegateFunction in eventDelegate.GetInvocationList()) {
                if (_applicationQuitFlag != 0) return -1;
                invocations += 1;

                if (delegateFunction.Target != null
                    && typeof(ISynchronizeInvoke).IsAssignableFrom(delegateFunction.Target.GetType())) {
                        try {
                            if ((delegateFunction.Target as ISynchronizeInvoke).InvokeRequired) {
                                (delegateFunction.Target as ISynchronizeInvoke).Invoke(delegateFunction, args);
                                invoked += 1;
                            } else {
                                delegateFunction.DynamicInvoke(args);
                                invoked += 1;
                            }
                        } catch { }
                } else {
                    delegateFunction.DynamicInvoke(args);
                    invoked += 1;
                }
            }
            return invocations - invoked;
        }

        #endregion

        #region CRC

        private static UInt16[] CRC16_CCITT_Table = new UInt16[256] {
            0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7, 0x8108, 0x9129, 0xa14a, 0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef,
            0x1231, 0x0210, 0x3273, 0x2252, 0x52b5, 0x4294, 0x72f7, 0x62d6, 0x9339, 0x8318, 0xb37b, 0xa35a, 0xd3bd, 0xc39c, 0xf3ff, 0xe3de,
            0x2462, 0x3443, 0x0420, 0x1401, 0x64e6, 0x74c7, 0x44a4, 0x5485, 0xa56a, 0xb54b, 0x8528, 0x9509, 0xe5ee, 0xf5cf, 0xc5ac, 0xd58d,
            0x3653, 0x2672, 0x1611, 0x0630, 0x76d7, 0x66f6, 0x5695, 0x46b4, 0xb75b, 0xa77a, 0x9719, 0x8738, 0xf7df, 0xe7fe, 0xd79d, 0xc7bc,
            0x48c4, 0x58e5, 0x6886, 0x78a7, 0x0840, 0x1861, 0x2802, 0x3823, 0xc9cc, 0xd9ed, 0xe98e, 0xf9af, 0x8948, 0x9969, 0xa90a, 0xb92b,
            0x5af5, 0x4ad4, 0x7ab7, 0x6a96, 0x1a71, 0x0a50, 0x3a33, 0x2a12, 0xdbfd, 0xcbdc, 0xfbbf, 0xeb9e, 0x9b79, 0x8b58, 0xbb3b, 0xab1a,
            0x6ca6, 0x7c87, 0x4ce4, 0x5cc5, 0x2c22, 0x3c03, 0x0c60, 0x1c41, 0xedae, 0xfd8f, 0xcdec, 0xddcd, 0xad2a, 0xbd0b, 0x8d68, 0x9d49,
            0x7e97, 0x6eb6, 0x5ed5, 0x4ef4, 0x3e13, 0x2e32, 0x1e51, 0x0e70, 0xff9f, 0xefbe, 0xdfdd, 0xcffc, 0xbf1b, 0xaf3a, 0x9f59, 0x8f78,
            0x9188, 0x81a9, 0xb1ca, 0xa1eb, 0xd10c, 0xc12d, 0xf14e, 0xe16f, 0x1080, 0x00a1, 0x30c2, 0x20e3, 0x5004, 0x4025, 0x7046, 0x6067,
            0x83b9, 0x9398, 0xa3fb, 0xb3da, 0xc33d, 0xd31c, 0xe37f, 0xf35e, 0x02b1, 0x1290, 0x22f3, 0x32d2, 0x4235, 0x5214, 0x6277, 0x7256,
            0xb5ea, 0xa5cb, 0x95a8, 0x8589, 0xf56e, 0xe54f, 0xd52c, 0xc50d, 0x34e2, 0x24c3, 0x14a0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
            0xa7db, 0xb7fa, 0x8799, 0x97b8, 0xe75f, 0xf77e, 0xc71d, 0xd73c, 0x26d3, 0x36f2, 0x0691, 0x16b0, 0x6657, 0x7676, 0x4615, 0x5634,
            0xd94c, 0xc96d, 0xf90e, 0xe92f, 0x99c8, 0x89e9, 0xb98a, 0xa9ab, 0x5844, 0x4865, 0x7806, 0x6827, 0x18c0, 0x08e1, 0x3882, 0x28a3,
            0xcb7d, 0xdb5c, 0xeb3f, 0xfb1e, 0x8bf9, 0x9bd8, 0xabbb, 0xbb9a, 0x4a75, 0x5a54, 0x6a37, 0x7a16, 0x0af1, 0x1ad0, 0x2ab3, 0x3a92,
            0xfd2e, 0xed0f, 0xdd6c, 0xcd4d, 0xbdaa, 0xad8b, 0x9de8, 0x8dc9, 0x7c26, 0x6c07, 0x5c64, 0x4c45, 0x3ca2, 0x2c83, 0x1ce0, 0x0cc1,
            0xef1f, 0xff3e, 0xcf5d, 0xdf7c, 0xaf9b, 0xbfba, 0x8fd9, 0x9ff8, 0x6e17, 0x7e36, 0x4e55, 0x5e74, 0x2e93, 0x3eb2, 0x0ed1, 0x1ef0
        };

        public static UInt16 CRC16_CCITT(ref byte[] bytes, int offset, int length) {
            UInt16 crc = 0;

            for (int i = offset; i < offset + length; i++) {
                crc = (UInt16)(CRC16_CCITT_Table[(crc >> 8 ^ bytes[i]) & 0xFF] ^ (crc << 8));
            }

            return crc;
        }

        #endregion
    }

    public class TimeoutTask {
        public delegate void TimeoutHandler(bool timedOut);
        public event TimeoutHandler TimeoutChanged;

        private System.Timers.Timer _timer = new System.Timers.Timer();
        private int _notInvokedCount = 0;

        public bool TimedOut {
            get { return _timedOut ?? true; }
        }
        private bool? _timedOut = null;
        private bool _notTimedOut = false;
        public void Unset() {
            _notTimedOut = true;

            if (_timedOut != false) {
                _firstHandle = true;
                _timedOut = false;
                _notInvokedCount = Core.RaiseEvent(TimeoutChanged, new object[] { TimedOut });
            }
        }

        public bool FirstHandle {
            get {
                if (!TimedOut && _firstHandle) {
                    _firstHandle = false;
                    return true;
                } else return false;
            }
        }
        private bool _firstHandle = false;

        public TimeoutTask(int interval) {
            _timer.Interval = interval;
            _timer.AutoReset = true;
            _timer.Elapsed += delegate {
                if (_timedOut != true || _notInvokedCount != 0) {
                    if (_notTimedOut) {
                        _notTimedOut = false;
                    } else {
                        _timedOut = true;
                        _notInvokedCount = Core.RaiseEvent(TimeoutChanged, new object[] { TimedOut });
                    }
                }
            };
            _timer.Start();
        }
    }

    public class PeriodicTask {
        public event EventHandler Tick;

        private System.Timers.Timer _timer = new System.Timers.Timer();

        public PeriodicTask(int interval) {
            _timer.Interval = interval;
            _timer.AutoReset = true;
            _timer.Elapsed += delegate {
                Core.RaiseEvent(Tick, new object[] { this, EventArgs.Empty });
            };
            _timer.Start();
        }
    }

    public struct UdpMessage {
        private byte[] _data;
        private IPEndPoint _endPoint;

        public UdpMessage(ref byte[] udpDatagram, IPEndPoint endPoint) {
            _data = udpDatagram;
            _endPoint = endPoint;
        }

        public UdpMessage(byte ip4, int port, int descriptor, params byte[] fields) {
            _data = StandardProtocolHeader(descriptor, fields.Length);
            for (int i = 0; i < fields.Length; i++) _data[DataOffset + i] = fields[i];
            _endPoint = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 100, ip4 }), port);
        }

        public UdpMessage(byte ip1, byte ip2, byte ip3, byte ip4, int port, int descriptor, params byte[] fields) {
            _data = StandardProtocolHeader(descriptor, fields.Length);
            for (int i = 0; i < fields.Length; i++) _data[DataOffset + i] = fields[i];
            _endPoint = new IPEndPoint(new IPAddress(new byte[] { ip1, ip2, ip3, ip4 }), port);
        }

        public int Descriptor { get { return _data.Length < 2 ? -1 : _data[0] | _data[1] << 8; } }

        public int Number {
            get { return _data.Length < 4 ? -1 : _data[2] | _data[3] << 8; }
            set { _data[2] = (byte)(value & 0xFF); _data[3] = (byte)((value >> 8) & 0xFF); }
        }

        public IPEndPoint EndPoint { get { return _endPoint; } }
        public int Length { get { return _data.Length; } }
        public byte[] Datagram { get { return _data; } }

        public static byte[] StandardProtocolHeader(int descriptor, int fieldsLength = 0) {
            var header = new byte[HeaderLength + fieldsLength];

            header[0] = (byte)(descriptor & 0xff);
            header[1] = (byte)((descriptor >> 8) & 0xff);

            return header;
        }

        public const int HeaderLength = 12;
        public const int HeaderLastByte = HeaderLength - 1;
        public const int DataOffset = HeaderLength;
    }

    public struct CanMessage {
        public int Id;
        public byte[] Data;
        public bool Extended;
        public bool Rtr;

        public CanMessage(int id, bool standard = false, bool rtr = false) {
            Id = id;
            Extended = !standard;
            Rtr = rtr;
            Data = null;
        }

        public CanMessage(int id, int dataLength, bool standard = false, bool rtr = false) {
            Id = id;
            Extended = !standard;
            Rtr = rtr;
            Data = new byte[dataLength];
        }

        public CanMessage(ref Ixxat.Vci3.Bal.Can.CanMessage ixxatMessage) {
            Id = (int)ixxatMessage.Identifier;
            Extended = ixxatMessage.ExtendedFrameFormat;
            Rtr = ixxatMessage.RemoteTransmissionRequest;
            Data = new byte[ixxatMessage.DataLength];
            for (int i = 0; i < ixxatMessage.DataLength; i++) Data[i] = ixxatMessage[i];
        }

        public CanMessage(Ixxat.Vci3.Bal.Can.CanMessage ixxatMessage) {
            Id = (int)ixxatMessage.Identifier;
            Extended = ixxatMessage.ExtendedFrameFormat;
            Rtr = ixxatMessage.RemoteTransmissionRequest;
            Data = new byte[ixxatMessage.DataLength];
            for (int i = 0; i < ixxatMessage.DataLength; i++) Data[i] = ixxatMessage[i];
        }

        public CanMessage(int priority, int typeOfLength, int descriptor, int source, int destination, bool standard = false, bool rtr = false) {
            Id = 0;
            Extended = !standard;
            Rtr = rtr;
            Data = null;

            Id_PrTdDeAiAp_Priority = priority;
            Id_PrTdDeAiAp_TypeOfLength = typeOfLength;
            Id_PrTdDeAiAp_Descriptor = descriptor;
            Id_PrTdDeAiAp_Source = source;
            Id_PrTdDeAiAp_Destination = destination;
        }

        public CanMessage(int priority, int descriptor, int source, int destination, bool standard = false, bool rtr = false) {
            Id = 0;
            Extended = !standard;
            Rtr = rtr;
            Data = null;

            Id_PrDeApAi_Priority = priority;
            Id_PrDeApAi_Descriptor = descriptor;
            Id_PrDeApAi_Source = source;
            Id_PrDeApAi_Destination = destination;
        }

        // ID PARTS

        public int Id_PrTdDeAiAp_Priority { get { return (Id >> 27) & 0x3; } set { Id &= 0x7FFFFFF; Id |= (value & 0x3) << 27; } }
        public int Id_PrTdDeAiAp_TypeOfLength { get { return (Id >> 25) & 0x3; } set { Id &= 0x19FFFFFF; Id |= (value & 0x3) << 25; } }
        public int Id_PrTdDeAiAp_Descriptor { get { return (Id >> 14) & 0x7FF; } set { Id &= 0x1E003FFF; Id |= (value & 0x7FF) << 14; } }
        public int Id_PrTdDeAiAp_Source { get { return (Id >> 7) & 0x7F; } set { Id &= 0x1FFFC07F; Id |= (value & 0x7F) << 7; } }
        public int Id_PrTdDeAiAp_Destination { get { return Id & 0x7F; } set { Id &= 0x1FFFFF80; Id |= value & 0x7F; } }

        public int Id_PrDeApAi_Priority { get { return (Id >> 24) & 0x1F; } set { Id &= 0xFFFFFF; Id |= (value & 0x1F) << 24; } }
        public int Id_PrDeApAi_Descriptor { get { return (Id >> 12) & 0xFFF; } set { Id &= 0x1F000FFF; Id |= (value & 0xFFF) << 12; } }
        public int Id_PrDeApAi_Destination { get { return (Id >> 6) & 0x3F; } set { Id &= 0x1FFFF03F; Id |= (value & 0x3F) << 6; } }
        public int Id_PrDeApAi_Source { get { return Id & 0x3F; } set { Id &= 0x1FFFFFC0; Id |= value & 0x3F; } }

        
        // DATA PARTS

        public Int16 Data_2bytes0 { get { return (Int16)(Data[0] | (Data[1] << 8)); } }
        public Int16 Data_2bytes1 { get { return (Int16)(Data[1] | (Data[2] << 8)); } }
        public Int16 Data_2bytes2 { get { return (Int16)(Data[2] | (Data[3] << 8)); } }
        public Int16 Data_2bytes3 { get { return (Int16)(Data[3] | (Data[4] << 8)); } }
        public Int16 Data_2bytes4 { get { return (Int16)(Data[4] | (Data[5] << 8)); } }
        public Int16 Data_2bytes5 { get { return (Int16)(Data[5] | (Data[6] << 8)); } }
        public UInt16 Data_2bytes6 { get { return (UInt16)(Data[6] | (Data[7] << 8)); } }

        public bool DataBitOn(int byteNumber, int bitNumber) {
            if (Data.Length < byteNumber) return false;

            return ((Data[byteNumber] >> bitNumber) & 0x1) == 1;
        }


        public Ixxat.Vci3.Bal.Can.CanMessage IxxatMessage {
            get {
                var ixxatMessage = new Ixxat.Vci3.Bal.Can.CanMessage();
                ixxatMessage.TimeStamp = 0;
                ixxatMessage.FrameType = Ixxat.Vci3.Bal.Can.CanMsgFrameType.Data;
                ixxatMessage.SelfReceptionRequest = false;
                ixxatMessage.ExtendedFrameFormat = Extended;
                ixxatMessage.RemoteTransmissionRequest = Rtr;
                ixxatMessage.Identifier = (uint)Id;
                if (Data == null) {
                    ixxatMessage.DataLength = 0;
                } else {
                    ixxatMessage.DataLength = (byte)Data.Length;
                    for (int i = 0; i < Data.Length; i++) ixxatMessage[i] = Data[i];
                }

                return ixxatMessage;
            }
        }
    }

    public struct RSMessage {
        public byte[] Data;

        public RSMessage(params byte[] dataBytes) {
            Data = dataBytes;
        }

        public RSMessage(byte id, params byte[] dataBytes) {
            Data = new byte[6 + dataBytes.Length];
            Data[0] = 0xAA;
            Data[1] = 0xAA;
            Data[2] = (byte)(Data.Length - 2);
            Data[3] = id;
            for (int i = 0; i < dataBytes.Length; i++) Data[i + 4] = dataBytes[i];

            var crc = Core.CRC16_CCITT(ref Data, 3, dataBytes.Length + 1);
            Data[Data.Length - 2] = (byte)(crc & 0xFF);
            Data[Data.Length - 1] = (byte)((crc >> 8) & 0xFF);
        }

        public RSMessage(string dataLine) {
            Data = Encoding.UTF8.GetBytes(dataLine);
        }

        public byte[] Buffer { get {
            return Data;
        } }

        public string Line { get {
            return Encoding.UTF8.GetString(Data);
        } }
    }
}
