namespace project {
    partial class MainWindow {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxAdres = new System.Windows.Forms.ComboBox();
            this.SvValAdres = new System.Windows.Forms.Label();
            this.buttonReadAdr = new System.Windows.Forms.Button();
            this.buttonDevOff = new System.Windows.Forms.Button();
            this.buttonDevOn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button_privod_stop = new System.Windows.Forms.Button();
            this.label_upr_ED1controlValue = new System.Windows.Forms.Label();
            this.trackBar_privod_ED1vnControl = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DVcheck = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.DevResponse = new Burevestnik.PultLabel();
            this.DSResponse3 = new Burevestnik.PultLabel();
            this.DSResponse4 = new Burevestnik.PultLabel();
            this.DSResponse2 = new Burevestnik.PultLabel();
            this.DSResponse1 = new Burevestnik.PultLabel();
            this.pultLabel_can = new Burevestnik.PultLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonState = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.SvValState = new Burevestnik.PultLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_privod_ED1vnControl)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxAdres);
            this.groupBox1.Controls.Add(this.pultLabel_can);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Связь (адрес)";
            // 
            // comboBoxAdres
            // 
            this.comboBoxAdres.FormattingEnabled = true;
            this.comboBoxAdres.Location = new System.Drawing.Point(155, 24);
            this.comboBoxAdres.Name = "comboBoxAdres";
            this.comboBoxAdres.Size = new System.Drawing.Size(121, 21);
            this.comboBoxAdres.TabIndex = 4;
            this.comboBoxAdres.Text = "0";
            this.comboBoxAdres.SelectedIndexChanged += new System.EventHandler(this.comboBoxAdres_SelectedIndexChanged);
            // 
            // SvValAdres
            // 
            this.SvValAdres.Location = new System.Drawing.Point(106, 23);
            this.SvValAdres.Margin = new System.Windows.Forms.Padding(0);
            this.SvValAdres.Name = "SvValAdres";
            this.SvValAdres.Size = new System.Drawing.Size(30, 23);
            this.SvValAdres.TabIndex = 12;
            this.SvValAdres.Text = "0";
            this.SvValAdres.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonReadAdr
            // 
            this.buttonReadAdr.Location = new System.Drawing.Point(13, 21);
            this.buttonReadAdr.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReadAdr.Name = "buttonReadAdr";
            this.buttonReadAdr.Size = new System.Drawing.Size(93, 25);
            this.buttonReadAdr.TabIndex = 11;
            this.buttonReadAdr.Text = "Запрос адреса";
            this.buttonReadAdr.UseVisualStyleBackColor = true;
            // 
            // buttonDevOff
            // 
            this.buttonDevOff.Location = new System.Drawing.Point(189, 22);
            this.buttonDevOff.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDevOff.Name = "buttonDevOff";
            this.buttonDevOff.Size = new System.Drawing.Size(70, 28);
            this.buttonDevOff.TabIndex = 12;
            this.buttonDevOff.Text = "DevOff";
            this.buttonDevOff.UseVisualStyleBackColor = true;
            // 
            // buttonDevOn
            // 
            this.buttonDevOn.Location = new System.Drawing.Point(35, 22);
            this.buttonDevOn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDevOn.Name = "buttonDevOn";
            this.buttonDevOn.Size = new System.Drawing.Size(70, 28);
            this.buttonDevOn.TabIndex = 11;
            this.buttonDevOn.Text = "DevOn";
            this.buttonDevOn.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "вращение:";
            // 
            // button_privod_stop
            // 
            this.button_privod_stop.Location = new System.Drawing.Point(184, 17);
            this.button_privod_stop.Margin = new System.Windows.Forms.Padding(0);
            this.button_privod_stop.Name = "button_privod_stop";
            this.button_privod_stop.Size = new System.Drawing.Size(75, 28);
            this.button_privod_stop.TabIndex = 9;
            this.button_privod_stop.Text = "Стоп";
            this.button_privod_stop.UseVisualStyleBackColor = true;
            // 
            // label_upr_ED1controlValue
            // 
            this.label_upr_ED1controlValue.Location = new System.Drawing.Point(246, 63);
            this.label_upr_ED1controlValue.Margin = new System.Windows.Forms.Padding(0);
            this.label_upr_ED1controlValue.Name = "label_upr_ED1controlValue";
            this.label_upr_ED1controlValue.Size = new System.Drawing.Size(30, 23);
            this.label_upr_ED1controlValue.TabIndex = 7;
            this.label_upr_ED1controlValue.Text = "0";
            this.label_upr_ED1controlValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBar_privod_ED1vnControl
            // 
            this.trackBar_privod_ED1vnControl.LargeChange = 1;
            this.trackBar_privod_ED1vnControl.Location = new System.Drawing.Point(6, 64);
            this.trackBar_privod_ED1vnControl.Margin = new System.Windows.Forms.Padding(0);
            this.trackBar_privod_ED1vnControl.Minimum = -10;
            this.trackBar_privod_ED1vnControl.Name = "trackBar_privod_ED1vnControl";
            this.trackBar_privod_ED1vnControl.Size = new System.Drawing.Size(239, 45);
            this.trackBar_privod_ED1vnControl.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "ответ:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DVcheck);
            this.groupBox3.Controls.Add(this.trackBar_privod_ED1vnControl);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label_upr_ED1controlValue);
            this.groupBox3.Controls.Add(this.button_privod_stop);
            this.groupBox3.Location = new System.Drawing.Point(12, 75);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(291, 145);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DriveSpeed";
            // 
            // DVcheck
            // 
            this.DVcheck.AutoSize = true;
            this.DVcheck.Location = new System.Drawing.Point(6, 24);
            this.DVcheck.Name = "DVcheck";
            this.DVcheck.Size = new System.Drawing.Size(73, 17);
            this.DVcheck.TabIndex = 11;
            this.DVcheck.Text = "циклично";
            this.DVcheck.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonDevOff);
            this.groupBox4.Controls.Add(this.buttonDevOn);
            this.groupBox4.Location = new System.Drawing.Point(12, 226);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(291, 59);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "DevOn / DevOff";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.SvValAdres);
            this.groupBox5.Controls.Add(this.buttonReadAdr);
            this.groupBox5.Location = new System.Drawing.Point(318, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(162, 57);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Ответ";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.DSResponse3);
            this.groupBox6.Controls.Add(this.DSResponse4);
            this.groupBox6.Controls.Add(this.DSResponse2);
            this.groupBox6.Controls.Add(this.DSResponse1);
            this.groupBox6.Location = new System.Drawing.Point(318, 75);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(162, 145);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Ответ";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.DevResponse);
            this.groupBox7.Location = new System.Drawing.Point(318, 226);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(162, 59);
            this.groupBox7.TabIndex = 14;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Ответ";
            // 
            // DevResponse
            // 
            this.DevResponse.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.DevResponse.BorderRadius = 2;
            this.DevResponse.Location = new System.Drawing.Point(13, 18);
            this.DevResponse.Margin = new System.Windows.Forms.Padding(0);
            this.DevResponse.Name = "DevResponse";
            this.DevResponse.SelectedCode = 0;
            this.DevResponse.SelectedState = false;
            this.DevResponse.Size = new System.Drawing.Size(136, 32);
            this.DevResponse.TabIndex = 7;
            this.DevResponse.Text = "0";
            // 
            // DSResponse3
            // 
            this.DSResponse3.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.DSResponse3.BorderRadius = 2;
            this.DSResponse3.Location = new System.Drawing.Point(13, 77);
            this.DSResponse3.Margin = new System.Windows.Forms.Padding(0);
            this.DSResponse3.Name = "DSResponse3";
            this.DSResponse3.SelectedCode = 0;
            this.DSResponse3.SelectedState = false;
            this.DSResponse3.Size = new System.Drawing.Size(136, 32);
            this.DSResponse3.TabIndex = 6;
            this.DSResponse3.Text = "0";
            // 
            // DSResponse4
            // 
            this.DSResponse4.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.DSResponse4.BorderRadius = 2;
            this.DSResponse4.Location = new System.Drawing.Point(13, 107);
            this.DSResponse4.Margin = new System.Windows.Forms.Padding(0);
            this.DSResponse4.Name = "DSResponse4";
            this.DSResponse4.SelectedCode = 0;
            this.DSResponse4.SelectedState = false;
            this.DSResponse4.Size = new System.Drawing.Size(136, 32);
            this.DSResponse4.TabIndex = 5;
            this.DSResponse4.Text = "0";
            // 
            // DSResponse2
            // 
            this.DSResponse2.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.DSResponse2.BorderRadius = 2;
            this.DSResponse2.Location = new System.Drawing.Point(13, 47);
            this.DSResponse2.Margin = new System.Windows.Forms.Padding(0);
            this.DSResponse2.Name = "DSResponse2";
            this.DSResponse2.SelectedCode = 0;
            this.DSResponse2.SelectedState = false;
            this.DSResponse2.Size = new System.Drawing.Size(136, 32);
            this.DSResponse2.TabIndex = 4;
            this.DSResponse2.Text = "0";
            // 
            // DSResponse1
            // 
            this.DSResponse1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.DSResponse1.BorderRadius = 2;
            this.DSResponse1.Location = new System.Drawing.Point(13, 17);
            this.DSResponse1.Margin = new System.Windows.Forms.Padding(0);
            this.DSResponse1.Name = "DSResponse1";
            this.DSResponse1.SelectedCode = 0;
            this.DSResponse1.SelectedState = false;
            this.DSResponse1.Size = new System.Drawing.Size(136, 32);
            this.DSResponse1.TabIndex = 3;
            this.DSResponse1.Text = "0";
            // 
            // pultLabel_can
            // 
            this.pultLabel_can.BackColor = System.Drawing.SystemColors.Control;
            this.pultLabel_can.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.pultLabel_can.BorderRadius = 2;
            this.pultLabel_can.BorderWidth = 3;
            this.pultLabel_can.Location = new System.Drawing.Point(6, 19);
            this.pultLabel_can.Margin = new System.Windows.Forms.Padding(0);
            this.pultLabel_can.Name = "pultLabel_can";
            this.pultLabel_can.SelectedCode = 0;
            this.pultLabel_can.SelectedState = false;
            this.pultLabel_can.Size = new System.Drawing.Size(128, 32);
            this.pultLabel_can.TabIndex = 3;
            this.pultLabel_can.Text = "CAN:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonState);
            this.groupBox2.Location = new System.Drawing.Point(12, 291);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 59);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "State";
            // 
            // buttonState
            // 
            this.buttonState.Location = new System.Drawing.Point(35, 22);
            this.buttonState.Margin = new System.Windows.Forms.Padding(0);
            this.buttonState.Name = "buttonState";
            this.buttonState.Size = new System.Drawing.Size(70, 28);
            this.buttonState.TabIndex = 11;
            this.buttonState.Text = "State";
            this.buttonState.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.SvValState);
            this.groupBox8.Location = new System.Drawing.Point(318, 291);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(162, 59);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Ответ";
            // 
            // SvValState
            // 
            this.SvValState.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.SvValState.BorderRadius = 2;
            this.SvValState.Location = new System.Drawing.Point(13, 18);
            this.SvValState.Margin = new System.Windows.Forms.Padding(0);
            this.SvValState.Name = "SvValState";
            this.SvValState.SelectedCode = 0;
            this.SvValState.SelectedState = false;
            this.SvValState.Size = new System.Drawing.Size(136, 32);
            this.SvValState.TabIndex = 7;
            this.SvValState.Text = "0";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 357);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Управление приводом 20П01";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_privod_ED1vnControl)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBar_privod_ED1vnControl;
        private Burevestnik.PultLabel DSResponse1;
        private System.Windows.Forms.Label label1;
        private Burevestnik.PultLabel pultLabel_can;
        private System.Windows.Forms.Label label_upr_ED1controlValue;
        private System.Windows.Forms.Button button_privod_stop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonReadAdr;
        private System.Windows.Forms.Button buttonDevOff;
        private System.Windows.Forms.Button buttonDevOn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox DVcheck;
        private System.Windows.Forms.Label SvValAdres;
        private System.Windows.Forms.ComboBox comboBoxAdres;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private Burevestnik.PultLabel DSResponse3;
        private Burevestnik.PultLabel DSResponse4;
        private Burevestnik.PultLabel DSResponse2;
        private System.Windows.Forms.GroupBox groupBox7;
        private Burevestnik.PultLabel DevResponse;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonState;
        private System.Windows.Forms.GroupBox groupBox8;
        private Burevestnik.PultLabel SvValState;
    }
}

