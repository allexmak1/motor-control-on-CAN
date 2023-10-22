using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Diagnostics;

namespace Burevestnik {
    class PultButton: PultControl {

        public override int SelectedCode {
            get {
                foreach (var indicatorColor in this.indicatorColors) if (indicatorColor.Pressed) return indicatorColor.Code;
                foreach (var indicatorColor in this.indicatorColors) if (indicatorColor.LastUnpressed) return indicatorColor.Code;
                return 0;
            }
            set {
                foreach (var indicatorColor in this.indicatorColors) {
                    indicatorColor.Pressed = indicatorColor.Code == value;
                }
                this.Invalidate();
            }
        }
        public override bool SelectedState {
            get {
                foreach (var indicatorColor in this.indicatorColors) if (indicatorColor.Pressed) return true;
                return false;
            }
            set {
                foreach (var indicatorColor in this.indicatorColors) indicatorColor.Pressed = value;
                this.Invalidate();
            }
        }

        [Description("Индикатор может зажигаться красным цветом")][DefaultValue(false)][Category("Буревестник")]
        public bool ColorRed { get { return this.colorRed; } set { this.colorRed = value; updateColors(); } }
        [Description("Индикатор может зажигаться зелёным цветом")][DefaultValue(false)][Category("Буревестник")]
        public bool ColorGreen { get { return this.colorGreen; } set { this.colorGreen = value; updateColors(); } }
        [Description("Индикатор может зажигаться жёлтым цветом")][DefaultValue(false)][Category("Буревестник")]
        public bool ColorYellow { get { return this.colorYellow; } set { this.colorYellow = value; updateColors(); } }
        private bool colorRed = false;
        private bool colorGreen = false;
        private bool colorYellow = false;

        [Description("Если установлено, при клике мыши индикатор можно только включить.")][DefaultValue(false)][Category("Буревестник")]
        public bool SwitchOnOnly { get { return this.switchOnOnly; } set { this.switchOnOnly = value; } }
        private bool switchOnOnly = false;

        public override void On() { ButtonPressed = true; }
        public override void Off() { ButtonPressed = false; }
        public override void Flick() {
            this.buttonPressed = false;
            animationBorder.Start(this.BorderPressedWidth, 0, 400);
        }

        /// <summary>Возвращает или устанавливает нажатое состояние клавиши</summary>
        [Browsable(false)]
        public bool ButtonPressed {
            get { return this.buttonPressed; }
            set {
                if (this.buttonPressed != value) {
                    this.buttonPressed = value;
                    animationBorder.Start(
                        this.buttonPressed ? this.BorderPressedWidth : 0,
                        100);
                }
            }
        }
        private bool buttonPressed  = false;

        /// <summary>Цвета индикатора</summary>
        [Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<IndicatorColor> IndicatorColors {
            get { return this.indicatorColors; }
            set {
                this.indicatorColors = value;
                updateIndicatorColorsRectangles();
            }
        }
        private List<IndicatorColor> indicatorColors = new List<IndicatorColor>();

        public PultButton() {
            this.Config();
        }

        private void Config() {
            this.BackColor = SystemColors.ControlLight;
            this.cornerRadius = 7;
            this.Padding = new Padding(this.BorderPressedWidth / 2);

            this.animationBorder.StartValue = this.buttonPressed ? this.borderPressedWidth : 0;
            this.animationBorder.Tick = delegate { this.Invalidate(); };
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            foreach (var indicatorColor in this.indicatorColors) {
                indicatorColor.MouseOver = indicatorColor.Rectangle.Contains(e.Location);
            }
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            foreach (var indicatorColor in this.indicatorColors) indicatorColor.MouseOver = false;
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            updateIndicatorColorsRectangles();
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            onClick(e);
            base.OnMouseClick(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            onClick(e);
            base.OnMouseDoubleClick(e);
        }
        private void onClick(MouseEventArgs e) {
            if (this.SelectedState && this.switchOnOnly) return;

            foreach (var indicatorColor in this.indicatorColors) {
                if (indicatorColor.MouseOver) {
                    indicatorColor.Pressed = !indicatorColor.Pressed;
                    if (!indicatorColor.Pressed) indicatorColor.LastUnpressed = true;
                } else {
                    indicatorColor.Pressed = false;
                    indicatorColor.LastUnpressed = false;
                }
            }
        }

        private void updateColors() {
            var colors = new List<IndicatorColor>();
            if (this.colorRed) colors.Add(IndicatorColor.Red);
            if (this.colorGreen) colors.Add(IndicatorColor.Green);
            if (this.colorYellow) colors.Add(IndicatorColor.Yellow);

            IndicatorColors = colors;
        }


        Counter animationBorder = new Counter(Counter.SegmentedInAfter5);


        // Graphics parameters
        [Description("Задаёт толщину границы нажатой кнопки (вызов On() или Flick()).")][DefaultValue(5)][Category("Буревестник")]
        public int BorderPressedWidth {
            get { return this.borderPressedWidth; }
            set {
                this.borderPressedWidth = value;
                this.Padding = new Padding(value / 2);
                this.Invalidate();
            }
        }
        private int borderPressedWidth = 5;

        /// <summary>Возвращает или устанавливает форму кнопки</summary>
        [Description("Форма кнопки")][DefaultValue(ButtonForm.Rectangle)][Category("Буревестник")]
        public ButtonForm ButtonType { get { return this.buttonType; } set { this.buttonType = value; } }
        private ButtonForm buttonType = ButtonForm.Rectangle;


        [Description("Задаёт радиус скругления внешней границы.")][DefaultValue(0)][Category("Буревестник")]
        public int CornerRadius { get { return this.cornerRadius; } set { this.cornerRadius = value; } }
        private int cornerRadius = 0;

        StringFormat stringFormat = new StringFormat() {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };


        [Description("Отключает скругление левых верхнего и нижнего углов.")][DefaultValue(false)][Category("Буревестник")]
        public bool NoRoundCornersLeft { get { return this.noRoundCornersLeft; } set { this.noRoundCornersLeft = value; } }
        private bool noRoundCornersLeft = false;


        [Description("Отключает скругление правых верхнего и нижнего углов.")][DefaultValue(false)][Category("Буревестник")]
        public bool NoRoundCornersRight { get { return this.noRoundCornersRight; } set { this.noRoundCornersRight = value; } }
        private bool noRoundCornersRight = false;

        private void updateIndicatorColorsRectangles() {
            if (this.indicatorColors.Count > 0) {
                var size = new Size(this.Width / this.indicatorColors.Count, this.Height);
                for (int x = 0; x < this.indicatorColors.Count; x++) {
                    this.indicatorColors[x].Rectangle = new Rectangle(new Point(x * size.Width, 0), size);
                }
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.DrawString(
                this.Text, 
                this.Font, 
                this.Enabled ? SystemBrushes.ControlText : SystemBrushes.GrayText, 
                this.ClientRectangle, 
                this.stringFormat);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) {
            var rect = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Left - Padding.Right, Height - Padding.Top - Padding.Bottom);
            GraphicsPath roundedPath;

            switch (this.buttonType) {
                case ButtonForm.Ellipse: {
                        roundedPath = new GraphicsPath();
                        roundedPath.AddEllipse(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                        break;
                    }

                default:
                case ButtonForm.Rectangle: {
                        roundedPath = RoundedRectPath(
                            rect,
                            this.noRoundCornersLeft ? 0 : this.cornerRadius,
                            this.noRoundCornersRight ? 0 : this.cornerRadius,
                            this.noRoundCornersRight ? 0 : this.cornerRadius,
                            this.noRoundCornersLeft ? 0 : this.cornerRadius,
                            true);
                        break;
                    }
            }

            // Fill with parent background
            using (var brush = new SolidBrush(Parent.BackColor)) {
                pevent.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            // Fill disabled or with indicator colors or with back color 
            pevent.Graphics.SetClip(roundedPath);
            if (!this.Enabled) {
                pevent.Graphics.FillRectangle(SystemBrushes.Control, this.ClientRectangle);
            } else if (this.indicatorColors.Count > 0) {
                foreach (var indicatorColor in this.indicatorColors) {
                    Color color;
                    if (indicatorColor.Pressed) {
                        color = indicatorColor.MouseOver ? ColorMultipliedBy(indicatorColor.ColorOn, 0.2F) : indicatorColor.ColorOn;
                    } else {
                        color = indicatorColor.MouseOver ? ColorMultipliedBy(indicatorColor.ColorOff, 0.5F) : indicatorColor.ColorOff;
                    }
                    using (var brush = new SolidBrush(color)) {
                        pevent.Graphics.FillRectangle(brush, indicatorColor.Rectangle);
                    }
                }
            } else {
                using (var brush = new SolidBrush(this.BackColor)) {
                    pevent.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
            pevent.Graphics.ResetClip();

            // Default border
            using (var pen = new Pen(ColorMultipliedBy(this.BackColor, -0.1F), 1)) {
                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                pevent.Graphics.DrawPath(pen, roundedPath);
            }

            // Selected color border
            if (this.Enabled) {
                for (int i = 0; i < this.indicatorColors.Count; i++) {
                    var indicatorColor = this.indicatorColors[i];
                    if (indicatorColor.Pressed) {
                        using (var pen = new Pen(ColorMultipliedBy(indicatorColor.ColorOn, -0.3F), 2)) {
                            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            pevent.Graphics.DrawPath(pen, roundedPath);
                        }
                        // Выделять границей отдельный цвет, а не всю кнопку
                        //var outlineRect = indicatorColor.Rectangle;
                        //outlineRect.Intersect(rect);
                        //using (var pen = new Pen(ColorWithBrightness(indicatorColor.ColorOn, -0.3F), borderWidth * 2)) {
                        //    int radiusW = i == 0 ? CornerRadius : 0;
                        //    int radiusE = i == this.indicatorColors.Count - 1 ? CornerRadius : 0;
                        //    using (var path = RoundedRectPath(outlineRect, radiusW, radiusE, radiusE, radiusW, true)) {
                        //        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        //        pevent.Graphics.DrawPath(pen, path);
                        //    }
                        //}
                        break;
                    }
                }
            }

            // Action border
            if (animationBorder.Value > 0) {
                using (var pen = new Pen(ColorMultipliedBy(this.BackColor, -0.6F), animationBorder.Value)) {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    pevent.Graphics.DrawPath(pen, roundedPath);
                }
            }
        }
    }

    enum ButtonForm {
        Rectangle, Ellipse
    }
}
