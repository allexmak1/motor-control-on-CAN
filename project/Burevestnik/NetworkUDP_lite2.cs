using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Burevestnik {

    /// <summary>
    /// <para>Приём сообщений:</para>
    /// <para><c>NetworkUDP.ReceivePort = 11111;</c> — назначить локальный порт для приёма сообщений.</para>
    /// <para><c>NetworkUDP.MessageReceived += delegate (IPEndPoint remoteEP, data[] udpDatagram) { ... };</c> — подписаться на событие пришедшего сообщения.</para>
    /// <para>Отправка сообщений:</para>
    /// <para><c>NetworkUDP.Send(message, remoteEP);</c> — отправка датаграммы на указанный адрес.</para>
    /// <para>Либо указать адрес удалённого устройства:</para>
    /// <para><c>NetworkUDP.Connect(remoteEP);</c></para>
    /// <para><c>NetworkUDP.Send(message);</c></para>
    /// </summary>
    class NetworkUDP {

        private static UdpClient _udpClient = null;

        /// <summary>
        /// Устанавливает локальный порт, на который принимаются сообщения. Приём начинается сразу после установки порта.
        /// </summary>
        public static void Config(int port) {
            _udpClient = new UdpClient(port);
            _udpClient.BeginReceive(new AsyncCallback(UdpReceiveCallback), null); // ??? null
        }

        // ОТПРАВКА СООБЩЕНИЙ

        /// <summary>
        /// Отправляет датаграмму по указанному адресу.
        /// </summary>
        /// <param name="udpDatagram">Датаграмма</param>
        /// <param name="remoteEP">Адрес на который отправить</param>
        public static void Send(UdpMessage message) {
            if (_udpClient != null) {
                message.Number = NextMessageNumber;
                _udpClient.BeginSend(message.Datagram, message.Length, message.EndPoint, new AsyncCallback(UdpSendCallback), null);
            }
        }

        private static void UdpSendCallback(IAsyncResult result) {
            try {
                _udpClient.EndSend(result);
            } catch {
                // !!! TODO catch errors
            }
        }

        private static UInt16 NextMessageNumber {
            get {
                _messageNumber = (UInt16)(_messageNumber >= UInt16.MaxValue ? 0 : _messageNumber + 1);
                return _messageNumber;
            }
        }
        private static UInt16 _messageNumber = UInt16.MaxValue;


        // ПРИЁМ СООБЩЕНИЙ


        public delegate void UdpMessageReceiveHandler(Burevestnik.UdpMessage message);
        /// <summary>
        /// Происходит при получении UDP сообщения.
        /// </summary>
        public static event UdpMessageReceiveHandler MessageReceived = delegate { };

        private static void UdpReceiveCallback(IAsyncResult result) {
            IPEndPoint remoteEP = null;

            try {
                byte[] udpDatagram = _udpClient.EndReceive(result, ref remoteEP);
                MessageReceived(new UdpMessage(ref udpDatagram, remoteEP));
            } catch {
                // !!! TODO catch
            } finally {
                _udpClient.BeginReceive(new AsyncCallback(UdpReceiveCallback), null);
            }
        }
    }
}
