using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Burevestnik {
    class PultLabel : PultControl {

        /// <summary>Возвращает или устанавливает ширину границы</summary>
        [Description("Ширина границы")][DefaultValue(1)][Category("Буревестник")]
        public int BorderWidth { get { return this.borderWidth; } set { this.borderWidth = value; } }
        private int borderWidth = 1;

        /// <summary>Возвращает или устанавливает радиус скругления границы</summary>
        [Description("Радиус скругления границы")][DefaultValue(0)][Category("Буревестник")]
        public int BorderRadius {
            get { return this.borderRadius; }
            set { this.borderRadius = value; }
        }
        private int borderRadius = 0;

        /// <summary>Возвращает или устанавливает цвет границы</summary>
        [Description("Цвет границы")][Category("Буревестник")]
        public Color BorderColor {
            get { return this.borderColor; }
            set { this.borderColor = value; this.Invalidate(); }
        }
        private Color borderColor = SystemColors.ControlDark;

        /// <summary>Возвращает или задает цвет фона, сопоставленный с этим элементом управления.</summary>
        public override Color BackColor {
            get { return base.BackColor; }
            set { base.BackColor = value; BorderColor = ControlPaint.Dark(value); }
        }

        /// <summary>Возвращает или задает текст, сопоставленный с этим элементом управления.</summary>
        public override string Text {
            get {  return base.Text; }
            set { base.Text = value; this.Invalidate(); }
        }

        StringFormat stringFormat = new StringFormat() {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) {
            using (var brush = new SolidBrush(Parent.BackColor)) {
                pevent.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            var rect = this.ClientRectangle;
            rect.Inflate(-this.borderRadius / 2 - 2, -this.borderRadius / 2 - 2);

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var path = RoundedRectPath(rect, this.borderRadius, true)) {
                if (Enabled) {
                    using (var brush = new SolidBrush(this.BackColor)) {
                        pevent.Graphics.FillPath(brush, path);
                    }
                }

                using (var pen = new Pen(Enabled ? this.borderColor : SystemColors.ControlDark, this.borderWidth)) {
                    pevent.Graphics.DrawPath(pen, path);
                }
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            var textRect = this.ClientRectangle;
            textRect.Inflate(-borderWidth / 2 - 10, -borderWidth / 2 - 10);

            e.Graphics.DrawString(
                this.Text, 
                this.Font, 
                Enabled ? SystemBrushes.ControlText : SystemBrushes.GrayText, 
                textRect, 
                this.stringFormat);
        }
    }
}
