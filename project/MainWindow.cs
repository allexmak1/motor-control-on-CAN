using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Burevestnik;

namespace project {
    public partial class MainWindow : Form {

        CanMessage messageADR0 = new CanMessage(0x8008A80, 3);
        CanMessage messageADR3 = new CanMessage(0x8008A83, 3);
        CanMessage messageADR4 = new CanMessage(0x8008A84, 3);
        CanMessage messageADR5 = new CanMessage(0x8008A85, 3);

        CanMessage messageDevOnADR0 = new CanMessage(0x1043CA80, 0);
        CanMessage messageDevOnADR3 = new CanMessage(0x1043CA83, 0);
        CanMessage messageDevOnADR4 = new CanMessage(0x1043CA84, 0);
        CanMessage messageDevOnADR5 = new CanMessage(0x1043CA85, 0);

        CanMessage messageDevOffADR0 = new CanMessage(0x10440A80, 0);
        CanMessage messageDevOffADR3 = new CanMessage(0x10440A83, 0);
        CanMessage messageDevOffADR4 = new CanMessage(0x10440A84, 0);
        CanMessage messageDevOffADR5 = new CanMessage(0x10440A85, 0);

        CanMessage messageReadADR0 = new CanMessage(0x8EACA80, 1);
        CanMessage messageReadADR3 = new CanMessage(0x8EACA83, 1);
        CanMessage messageReadADR4 = new CanMessage(0x8EACA84, 1);
        CanMessage messageReadADR5 = new CanMessage(0x8EACA85, 1);

        CanMessage messageWriteADR0 = new CanMessage(0x8EB4A80, 2);
        CanMessage messageWriteADR3 = new CanMessage(0x8EB4A83, 2);
        CanMessage messageWriteADR4 = new CanMessage(0x8EB4A84, 2);
        CanMessage messageWriteADR5 = new CanMessage(0x8EB4A85, 2);

        CanMessage messageStateADR0 = new CanMessage(0x10404A80, 0);
        CanMessage messageStateADR3 = new CanMessage(0x10404A84, 0);
        CanMessage messageStateADR4 = new CanMessage(0x10404A83, 0);
        CanMessage messageStateADR5 = new CanMessage(0x10404A85, 0);
        int SelectAdresUstr;
        string bufferText;

        int Cod;
        int OutSpeed;
        int InSpeed;
        int Cod2;

        public MainWindow() {
            InitializeComponent();

            comboBoxAdres.Items.AddRange(new string[] { "0", "3 - взвод", "4 - выбор ленты", "5 - предохранитель", "все"});

            NetworkCANixxat.ConfigPorts(250);

            NetworkCANixxat.MessageReceived += delegate(NetworkCANixxat.CanSocket socket, Burevestnik.CanMessage message) {
                Core.TimeoutCheck(message.Id);

                switch (message.Id) {
                    //DriveSpeed
                    case 0x1000C015://0 адрес
                        if (SelectAdresUstr == 0) {
                            Cod = message.Data_2bytes0 & 0xFF;
                            DSResponse1.Text = String.Format("{0:X}", Cod);
                            DSResponse1.Text += addMesCodeCmand(Cod);

                            //как есть 0478
                            OutSpeed = message.Data_2bytes1;
                            //перевернули 7804
                            //OutSpeed = (message.Data_2bytes1 & 0xFF)<<8;
                            //OutSpeed |= (message.Data_2bytes1 & 0xFF00) >> 8;
                            DSResponse2.Text = String.Format("{0}", OutSpeed);

                            InSpeed = message.Data_2bytes3;
                            DSResponse3.Text = String.Format("{0}", InSpeed);

                            Cod2 = message.Data_2bytes5 & 0xFF;
                            DSResponse4.Text = String.Format("{0:X}", Cod2);
                        }
                        break;
                    case 0x1000C195://3 адрес
                        if (SelectAdresUstr == 3) {
                            Cod = message.Data_2bytes0 & 0xFF;
                            DSResponse1.Text = String.Format("{0:X}", Cod);
                            DSResponse1.Text += addMesCodeCmand(Cod);

                            OutSpeed = message.Data_2bytes1;
                            DSResponse2.Text = String.Format("{0}", OutSpeed);

                            InSpeed = message.Data_2bytes3;
                            DSResponse3.Text = String.Format("{0}", InSpeed);

                            Cod2 = message.Data_2bytes5 & 0xFF;
                            DSResponse4.Text = String.Format("{0:X}", Cod2);
                        }
                        break;
                    case 0x1000C215://4 адрес
                        if (SelectAdresUstr == 4) {
                            Cod = message.Data_2bytes0 & 0xFF;
                            DSResponse1.Text = String.Format("{0:X}", Cod);
                            DSResponse1.Text += addMesCodeCmand(Cod);

                            OutSpeed = message.Data_2bytes1;
                            DSResponse2.Text = String.Format("{0}", OutSpeed);

                            InSpeed = message.Data_2bytes3;
                            DSResponse3.Text = String.Format("{0}", InSpeed);

                            Cod2 = message.Data_2bytes5 & 0xFF;
                            DSResponse4.Text = String.Format("{0:X}", Cod2);
                        }
                        break;
                    case 0x1000C295://5 адрес
                        if (SelectAdresUstr == 5) {
                            Cod = message.Data_2bytes0 & 0xFF;
                            DSResponse1.Text = String.Format("{0:X}", Cod);
                            DSResponse1.Text += addMesCodeCmand(Cod);

                            OutSpeed = message.Data_2bytes1;
                            DSResponse2.Text = String.Format("{0}", OutSpeed);

                            InSpeed = message.Data_2bytes3;
                            DSResponse3.Text = String.Format("{0}", InSpeed);

                            Cod2 = message.Data_2bytes5 & 0xFF;
                            DSResponse4.Text = String.Format("{0:X}", Cod2);
                        }
                        break;
                    //DevON/OFF
                    case 0x18438015://0 адрес
                        if (SelectAdresUstr == 0) {
                            Cod = message.Data_2bytes0;
                            if (Cod == 0x010F) DevResponse.Text = "DevON";
                            if (Cod == 0x0110) DevResponse.Text = "DevOFF";
                        }
                        break;
                    case 0x18438195://3 адрес
                        if (SelectAdresUstr == 3) {
                            Cod = message.Data_2bytes0;
                            if (Cod == 0x010F) DevResponse.Text = "DevON";
                            if (Cod == 0x0110) DevResponse.Text = "DevOFF";
                        }
                        break;
                    case 0x18438215://4 адрес
                        if (SelectAdresUstr == 4) {
                            Cod = message.Data_2bytes0;
                            if (Cod == 0x010F) DevResponse.Text = "DevON";
                            if (Cod == 0x0110) DevResponse.Text = "DevOFF";
                        }
                        break;
                    case 0x18438295://5 адрес
                        if (SelectAdresUstr == 5) {
                            Cod = message.Data_2bytes0;
                            if (Cod == 0x010F) DevResponse.Text = "DevON";
                            if (Cod == 0x0110) DevResponse.Text = "DevOFF";
                        }
                        break;

                    //ReadParam
                    case 0x8EB0015://0 адрес
                        SvValAdres.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        break;
                    case 0x8EB0195://3 адрес
                        SvValAdres.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        break;
                    case 0x8EB0215://4 адрес
                        SvValAdres.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        break;
                    case 0x8EB0295://5 адрес
                        SvValAdres.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        break;

                    //State
                    case 0x112A0015://0 адрес
                        SvValState.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        break;
                    case 0x112A0195://3 адрес
                        SvValState.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        break;
                    case 0x112A0215://4 адрес
                        SvValState.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        break;
                    case 0x112A0295://5 адрес
                        SvValState.Text = (message.Data_2bytes0 & 0xFF).ToString();
                        //Cod2 = message.Data_2bytes5 & 0xFF;
                        //DSResponse4.Text = String.Format("{0:X}", Cod2);
                        break;
                }
            };

            /*
            Core.TimeoutStart(1000, 0x112A0215, delegate(bool timedOut) {
            };*/

            //периодическая отправка
            Core.PeriodicTaskStart(200, delegate {

                UpdateCanLabelState(pultLabel_can, false);

                //отправка DriveSpeed
                if (DVcheck.Checked) {
                    int ed1valOut = map(trackBar_privod_ED1vnControl.Value, -10, 10, -8738, 8738);
                    if (trackBar_privod_ED1vnControl.Value == 1) { ed1valOut = 130; }
                    if (trackBar_privod_ED1vnControl.Value == -1) { ed1valOut = -130; }
                    switch (SelectAdresUstr) {
                        case 0:
                            messageADR0.Data[0] = (byte)(ed1valOut);
                            messageADR0.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR0.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR0);
                            break;
                        case 3:
                            messageADR3.Data[0] = (byte)(ed1valOut);
                            messageADR3.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR3.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR3);
                            break;
                        case 4:
                            messageADR4.Data[0] = (byte)(ed1valOut);
                            messageADR4.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR4.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR4);
                            break;
                        case 5:
                            messageADR5.Data[0] = (byte)(ed1valOut);
                            messageADR5.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR5.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR5);
                            break;
                        case 6:
                            messageADR0.Data[0] = (byte)(ed1valOut);
                            messageADR0.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR0.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR0);
                            messageADR3.Data[0] = (byte)(ed1valOut);
                            messageADR3.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR3.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR3);
                            messageADR4.Data[0] = (byte)(ed1valOut);
                            messageADR4.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR4.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR4);
                            messageADR5.Data[0] = (byte)(ed1valOut);
                            messageADR5.Data[1] = (byte)(ed1valOut >> 8);
                            messageADR5.Data[2] = 0x01;
                            NetworkCANixxat.Send(1, messageADR5);
                            break;
                    }
                }
            });

            //val ползунка
            trackBar_privod_ED1vnControl.ValueChanged += delegate {
                label_upr_ED1controlValue.Text = trackBar_privod_ED1vnControl.Value.ToString();
                /**/
                int ed1valOut = map(trackBar_privod_ED1vnControl.Value, -10, 10, -8738, 8738);
                if (trackBar_privod_ED1vnControl.Value == 1) { ed1valOut = 130; }
                if (trackBar_privod_ED1vnControl.Value == -1) { ed1valOut = -130; }
                switch (SelectAdresUstr) {
                    case 0:
                        messageADR0.Data[0] = (byte)(ed1valOut);
                        messageADR0.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR0.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR0);
                        break;
                    case 3:
                        messageADR3.Data[0] = (byte)(ed1valOut);
                        messageADR3.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR3.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR3);
                        break;
                    case 4:
                        messageADR4.Data[0] = (byte)(ed1valOut);
                        messageADR4.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR4.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR4);
                        break;
                    case 5:
                        messageADR5.Data[0] = (byte)(ed1valOut);
                        messageADR5.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR5.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR5);
                        break;
                    case 6:
                        messageADR0.Data[0] = (byte)(ed1valOut);
                        messageADR0.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR0.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR0);
                        messageADR3.Data[0] = (byte)(ed1valOut);
                        messageADR3.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR3.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR3);
                        messageADR4.Data[0] = (byte)(ed1valOut);
                        messageADR4.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR4.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR4);
                        messageADR5.Data[0] = (byte)(ed1valOut);
                        messageADR5.Data[1] = (byte)(ed1valOut >> 8);
                        messageADR5.Data[2] = 0x01;
                        NetworkCANixxat.Send(1, messageADR5);
                        break;
                }
            };

            //кнопка стоп
            button_privod_stop.MouseClick += delegate {
                trackBar_privod_ED1vnControl.Value = 0;
            };

            //кнопка Devon
            buttonDevOn.MouseClick += delegate {
                switch (SelectAdresUstr) {
                    case 0:
                        NetworkCANixxat.Send(1, messageDevOnADR0);
                        break;
                    case 3:
                        NetworkCANixxat.Send(1, messageDevOnADR3);
                        break;
                    case 4:
                        NetworkCANixxat.Send(1, messageDevOnADR4);
                        break;
                    case 5:
                        NetworkCANixxat.Send(1, messageDevOnADR5);
                        break;
                    case 6:
                        NetworkCANixxat.Send(1, messageDevOnADR0);
                        NetworkCANixxat.Send(1, messageDevOnADR3);
                        NetworkCANixxat.Send(1, messageDevOnADR4);
                        NetworkCANixxat.Send(1, messageDevOnADR5);
                        break;
                }
                trackBar_privod_ED1vnControl.Value = 0;
            };
            //кнопка Devoff
            buttonDevOff.MouseClick += delegate {
                switch (SelectAdresUstr) {
                    case 0:
                        NetworkCANixxat.Send(1, messageDevOffADR0);
                        break;
                    case 3:
                        NetworkCANixxat.Send(1, messageDevOffADR3);
                        break;
                    case 4:
                        NetworkCANixxat.Send(1, messageDevOffADR4);
                        break;
                    case 5:
                        NetworkCANixxat.Send(1, messageDevOffADR5);
                        break;
                    case 6:
                        NetworkCANixxat.Send(1, messageDevOffADR0);
                        NetworkCANixxat.Send(1, messageDevOffADR3);
                        NetworkCANixxat.Send(1, messageDevOffADR4);
                        NetworkCANixxat.Send(1, messageDevOffADR5);
                        break;
                }
                trackBar_privod_ED1vnControl.Value = 0;
            };

            //кнопка чтения адреса КЭН
            buttonReadAdr.MouseClick += delegate {
                messageReadADR0.Data[0] = 98;
                messageReadADR3.Data[0] = 98;
                messageReadADR4.Data[0] = 98;
                messageReadADR5.Data[0] = 98;
                NetworkCANixxat.Send(1, messageReadADR0);
                NetworkCANixxat.Send(1, messageReadADR3);
                NetworkCANixxat.Send(1, messageReadADR4);
                NetworkCANixxat.Send(1, messageReadADR5);
            };

            //кнопка состояния State
            buttonState.MouseClick += delegate {
                switch (SelectAdresUstr) {
                    case 0:
                        NetworkCANixxat.Send(1, messageStateADR0);
                        break;
                    case 3:
                        NetworkCANixxat.Send(1, messageStateADR3);
                        break;
                    case 4:
                        NetworkCANixxat.Send(1, messageStateADR4);
                        break;
                    case 5:
                        NetworkCANixxat.Send(1, messageStateADR5);
                        break;
                    case 6:
                        NetworkCANixxat.Send(1, messageStateADR0);
                        NetworkCANixxat.Send(1, messageStateADR3);
                        NetworkCANixxat.Send(1, messageStateADR4);
                        NetworkCANixxat.Send(1, messageStateADR5);
                        break;
                }
            };

            //кнопка записи адреса ! неработает !
            //в ответе AD03(941)CA(должно быть 36)0000
            /*buttonWriteAdr.MouseClick += delegate {
                int val = Convert.ToByte(textBoxADR.Text);
                if (val == 0 || val == 3 || val == 4 || val == 5) {
                    messageWriteADR0.Data[0] = (byte)(val);
                    messageWriteADR0.Data[1] = 98;
                    NetworkCANixxat.Send(messageWriteADR0);
                } else {
                    textBoxADR.Text = "Недопустимо";
                }
            };*/

        }


        //удалить
        /*private void UpdatePrivodCanState(bool timedOut) {
            var disableControls = UpdateCanLabelState(pultLabel_can, timedOut);
        }*/
        private string addMesCodeCmand(int code) {
            string buff;
            switch(code){
                case 0x36:
                    buff = " команда выполнена";
                    break;
                case 0x35:
                    buff = " команда не выполнена";
                    break;
                case 0xCB:
                    buff = " неверная команда";
                    break;
                case 0xCE:
                    buff = " неверная длинна данных";
                    break;
                case 0xCC:
                    buff = " данные вне диапазона";
                    break;
                default:
                    buff = " ???";
                    break;
            }
            return buff;
        }
        //ф-я масштабирования
        private int map(int x, int in_min, int in_max, int out_min, int out_max) {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }


        private bool UpdateCanLabelState(PultLabel canLabel, bool timedOut) {
            var socket = NetworkCANixxat.LastActiveSocket;
            var disableControls = false;

            if (socket == null) {
                canLabel.BackColor = PultControl.ColorBad;
                canLabel.Text = "CAN: нет IXXAT";
                disableControls = true;
            } else {
                switch (socket.State) {
                    case NetworkCANixxat.CanSocketState.Working:
                    case NetworkCANixxat.CanSocketState.WorkingNoMessages:
                        if (timedOut) {
                            //canLabel.BackColor = PultControl.ColorNotGood;
                            canLabel.BackColor = PultControl.DefaultBackColor;
                            canLabel.Text = "CAN: не в сети";
                            disableControls = true;
                        } else {
                            //canLabel.BackColor = PultControl.ColorGood;
                            canLabel.BackColor = PultControl.DefaultBackColor;
                            canLabel.Text = "CAN: в сети";
                        }
                        break;
                    default:
                        canLabel.BackColor = PultControl.ColorBad;
                        canLabel.Text = "CAN: " + socket.StateTextSimple;
                        disableControls = true;
                        break;
                }
            }

            return disableControls;
        }

        private void comboBoxAdres_SelectedIndexChanged(object sender, EventArgs e) {
                if(comboBoxAdres.SelectedIndex == 0){
                    SelectAdresUstr = 0;
                } else if (comboBoxAdres.SelectedIndex == 1) {
                    SelectAdresUstr = 3;
                } else if (comboBoxAdres.SelectedIndex == 2) {
                    SelectAdresUstr = 4;
                } else if (comboBoxAdres.SelectedIndex == 3) {
                    SelectAdresUstr = 5;
                } else if (comboBoxAdres.SelectedIndex == 4) {
                    SelectAdresUstr = 6;
                }
        }
    }
}

