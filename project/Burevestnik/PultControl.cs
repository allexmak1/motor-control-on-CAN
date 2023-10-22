using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Burevestnik {
    class PultControl: Control {

        /// <summary>Нажать клавишу или включить тумблер</summary>
        public virtual void On() { }
        /// <summary>Отжать клавишу или включить тумблер</summary>
        public virtual void Off() { }
        /// <summary>Моргнуть клавишей</summary>
        public virtual void Flick() { }

        /// <summary>Возвращает или устанавливает выбраный код индикатора</summary>
        [Browsable(false)]
        public virtual int SelectedCode { get; set; }
        /// <summary>Возвращает или устанавливает состояние вкл/выкл индикатора</summary>
        [Browsable(false)]
        public virtual bool SelectedState { get; set; }

        /// <summary>
        /// Возвращает или устанавливает код клавиши пульта
        /// </summary>
        [Description("Код клавиши пульта")][DefaultValue(-1)]
        [Category("Буревестник")]
        public int ProtocolButtonNumber {
            get { return this.protocolButtonNumber; } 
            set {
                if (PultButtons.ContainsKey(this.protocolButtonNumber)) { 
                    PultButtons.Remove(this.protocolButtonNumber); 
                }
                this.protocolButtonNumber = value;
                if (this.protocolButtonNumber > 0) {
                    PultButtons.Add(this.protocolButtonNumber, this);
                }
            } 
        }
        protected int protocolButtonNumber = -1;

        /// <summary>
        /// Возвращает или устанавливает код цветового табло пульта
        /// </summary>
        [Description("Код цветового табло пульта")][DefaultValue(-1)]
        [Category("Буревестник")]
        public int ProtocolIndicatorNumber { 
            get { return this.protocolIndicatorNumber; } 
            set {
                if (PultIndicators.ContainsKey(this.protocolIndicatorNumber)) {
                    this.MouseClick -= PultIndicator_MouseClick;
                    this.MouseDoubleClick -= PultIndicator_MouseClick;
                    PultIndicators.Remove(this.protocolIndicatorNumber);
                }
                this.protocolIndicatorNumber = value;
                if (this.protocolIndicatorNumber > 0) {
                    this.MouseClick += PultIndicator_MouseClick;
                    this.MouseDoubleClick += PultIndicator_MouseClick;
                    PultIndicators.Add(this.protocolIndicatorNumber, this);
                }
            } 
        }
        protected int protocolIndicatorNumber = -1;

        /// <summary>
        /// Возвращает или устанавливает смещение в массиве состояния пульта
        /// </summary>
        [Description("Смещение в массиве состояния пульта")][DefaultValue(-1)]
        [Category("Буревестник")]
        public int ProtocolOffset {
            get { return this.protocolOffset; }
            set { this.protocolOffset = value; }
        }
        protected int protocolOffset = -1;


        public PultControl() {
            DoubleBuffered = true;
            this.Enabled = true;
            PultControl.PultControls.Add(this);
        }


        // STATIC

        /// <summary>Все клавиши пульта по кодам</summary>
        public static Dictionary<int, PultControl> PultButtons = new Dictionary<int,PultControl>();
        /// <summary>Все индикаторы пульта по кодам</summary>
        public static Dictionary<int, PultControl> PultIndicators = new Dictionary<int,PultControl>();
        /// <summary>Все контролы пульта</summary>
        public static List<PultControl> PultControls = new List<PultControl>();

        public delegate void MouseClickHandler(PultControl indicator);
        public static event MouseClickHandler MouseClickOnIndicator = delegate { };

        public static void PultIndicator_MouseClick(object sender, MouseEventArgs e) {
            var indicator = sender as PultControl;
            if (indicator != null && indicator.ProtocolIndicatorNumber >= 0) {
                PultControl.MouseClickOnIndicator(indicator);
            }
        }

        public static Color ColorGood { get { return Color.FromArgb(163, 169, 72); } }
        public static Color ColorNotGood { get { return Color.FromArgb(237, 185, 46); } }
        public static Color ColorBad { get { return Color.FromArgb(248, 89, 49); } }

        public static GraphicsPath RoundedRectPath(RectangleF rectangle, int radius, bool isControlBounds = false) {
            return RoundedRectPath(new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height), radius, radius, radius, radius, isControlBounds);
        }

        public static GraphicsPath RoundedRectPath(Rectangle rectangle, int radius, bool isControlBounds = false) {
            return RoundedRectPath(rectangle, radius, radius, radius, radius, isControlBounds);
        }

        public static GraphicsPath RoundedRectPath(Rectangle rectangle, int radiusNW, int radiusNE, int radiusSE, int radiusSW, bool isControlBounds = false) {
            var path = new GraphicsPath();
            var margin = isControlBounds ? 1 : 0;

            if (radiusNW == 0 && radiusNE == 0 && radiusSE == 0 && radiusSW == 0) {
                path.AddRectangle(new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width - margin, rectangle.Height - margin));
            } else {
                var angleRect = Rectangle.Empty;

                path.StartFigure();
                if (radiusNW == 0) {
                    path.AddLine(rectangle.Left, rectangle.Top, rectangle.Left + 1, rectangle.Top);
                } else {
                    angleRect.Size = new Size(radiusNW * 2, radiusNW * 2);
                    angleRect.Location = new Point(rectangle.Left, rectangle.Top);
                    path.AddArc(angleRect, 180, 90);
                }
                if (radiusNE == 0) {
                    path.AddLine(rectangle.Right - margin, rectangle.Top, rectangle.Right - margin, rectangle.Top + 1);
                } else {
                    angleRect.Size = new Size(radiusNE * 2, radiusNE * 2);
                    angleRect.Location = new Point(rectangle.Right - angleRect.Width - margin, rectangle.Top);
                    path.AddArc(angleRect, 270, 90);
                }
                if (radiusSE == 0) {
                    path.AddLine(rectangle.Right - margin, rectangle.Bottom - margin, rectangle.Right - margin - 1, rectangle.Bottom - margin);
                } else {
                    angleRect.Size = new Size(radiusSE * 2, radiusSE * 2);
                    angleRect.Location = new Point(rectangle.Right - angleRect.Width - margin, rectangle.Bottom - angleRect.Height - margin);
                    path.AddArc(angleRect, 0, 90);
                }
                if (radiusSW == 0) {
                    path.AddLine(rectangle.Left, rectangle.Bottom - margin, rectangle.Left, rectangle.Bottom - margin - 1);
                } else {
                    angleRect.Size = new Size(radiusSW * 2, radiusSW * 2);
                    angleRect.Location = new Point(rectangle.Left, rectangle.Bottom - angleRect.Height - margin);
                    path.AddArc(angleRect, 90, 90);
                }
                path.CloseFigure();
            }

            return path;
        }

        public static Color ColorMultipliedBy(Color color, float correctionFactor) {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0) {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            } else {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
    }


    // ВСПОМОГАТЕЛЬНЫЕ СТРУКТУРЫ И КЛАССЫ

    public struct PointCZ {
        public float C;
        public float Z;

        /// <summary>
        /// Точка с координатой и высотой.
        /// </summary>
        /// <param name="c">Координата по оси x или y</param>
        /// <param name="z">Высота точки</param>
        public PointCZ(float c, float z) {
            this.Z = z;
            this.C = c;
        }

        public void Rotate(PointCZ axis, float degrees) {
            var sin = Math.Sin(degrees * Math.PI / 180);
            var cos = Math.Cos(degrees * Math.PI / 180);

            this.Z -= axis.Z;
            this.C -= axis.C;

            var z = this.Z * cos - this.C * sin;
            var c = this.Z * sin + this.C * cos;

            this.Z = (float)z + axis.Z;
            this.C = (float)c + axis.C;
        }

        public float AngleTo(PointCZ point) {
            return (float)(Math.Atan2(Math.Abs(point.Z - this.Z), Math.Abs(point.C - this.C)) * 180 / Math.PI);
        }
    }

    public struct PointXYZ {
        public float X;
        public float Y;
        public float Z;

        public PointXYZ(float x, float y, float z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void RotateAroundX(PointXYZ axis, float angle) {
            Rotate(ref this.Y, ref this.Z, axis.Y, axis.Z, angle);
        }

        public void RotateAroundY(PointXYZ axis, float angle) {
            Rotate(ref this.Z, ref this.X, axis.Z, axis.X, angle);
        }

        public void RotateAroundZ(PointXYZ axis, float angle) {
            Rotate(ref this.X, ref this.Y, axis.X, axis.Y, angle);
        }

        public static void Rotate(ref float p1, ref float p2, float a1, float a2, float angle) {
            var sin = Math.Sin(angle * Math.PI / 180);
            var cos = Math.Cos(angle * Math.PI / 180);

            p1 -= a1;
            p2 -= a2;

            var tp1 = p1 * cos - p2 * sin;
            var tp2 = p1 * sin + p2 * cos;

            p1 = (float)tp1 + a1;
            p2 = (float)tp2 + a2;
        }
    }

    public class Counter {
        public float Value = 0;
        public bool Active {
            get { return this.timerTick.Enabled; }
        }

        /// <summary>
        /// Функция скорости изменения величины.
        /// </summary>
        /// <param name="progress">Прогресс анимации от 0 до 1</param>
        public delegate double EasingFunction(float progress);
        public EasingFunction EasingFunctionDelegate = delegate { return 1; };
        public EventHandler Tick = delegate { };

        public float StartValue {
            get { return this.startValue; }
            set {
                this.startValue = value;
                this.Value = (int)value;
            }
        }
        public float startValue = 0;
        private float targetValue = 0;
        private float currentValue = 0;
        private float duration = 1000;
        private System.Timers.Timer timerTick = new System.Timers.Timer(10); // works like 60 fps tick
        private System.Timers.Timer timerDelay = new System.Timers.Timer();

        public Counter() {
            Config();
        }

        public Counter(EasingFunction easingFunction) {
            Config();
            this.EasingFunctionDelegate = easingFunction;
        }

        private void Config() {
            this.timerTick.AutoReset = true;
            this.timerTick.Elapsed += delegate {
                updateValue();
            };

            this.timerDelay.AutoReset = false;
            this.timerDelay.Elapsed += delegate {
                updateValue();
                this.timerTick.Start();
            };
        }

        public void Start(float to, int milliseconds, int delay = 0) {
            Start(Value, to, milliseconds, delay);
        }
        public void Start(float from, float to, int milliseconds, int delay = 0) {
            this.timerTick.Stop();

            this.currentValue = from;
            this.StartValue = from;
            this.targetValue = to;
            this.duration = milliseconds;
            if (to != from) {
                if (delay == 0 || this.timerDelay.Enabled) {
                    updateValue();
                    this.timerDelay.Stop();
                    this.timerTick.Start();
                } else {
                    this.timerDelay.Interval = delay;
                    this.timerDelay.Start();
                }
            }
        }

        private void updateValue() {
            this.currentValue += ((1000F / 100) * (targetValue - this.startValue)) / this.duration;

            double progress = Math.Abs(EasingFunctionDelegate((currentValue - this.startValue) / (targetValue - this.startValue)));

            if (progress >= 1 || progress < 0 || double.IsNaN(progress)) {
                timerTick.Stop();
                progress = 1;
            }

            Value = (int)(this.startValue + (targetValue - this.startValue) * progress);

            Tick(this, EventArgs.Empty);
        }

        public static EasingFunction Linear {
            get {
                return delegate(float progress) {
                    return progress;
                };
            }
        }

        public static EasingFunction SegmentedInAfter5 {
            get {
                return delegate(float progress) {
                    return progress < 0.5F ? 0 : 2 * progress - 1;
                };
            }
        }

        public static EasingFunction SegmentedInOutQuartWithStep4 {
            get {
                return delegate(float progress) {
                    if (progress < 0.3F) {
                        return progress / 6;
                    } else if (progress < 0.6F) {
                        return progress - 0.25F;
                    } else if (progress < 0.9F) {
                        return (progress - 0.9F) / 6 + 0.4;
                    } else {
                        return 6 * progress - 5;
                    }
                };
            }
        }

        public static EasingFunction EaseInCubic {
            get {
                return delegate(float progress) {
                    return Math.Pow(progress, 3);
                };
            }
        }

        public static EasingFunction EaseOutCubic {
            get {
                return delegate(float progress) {
                    return 1 - Math.Pow(1 - progress, 3);
                };
            }
        }

        public static EasingFunction EaseInOutCubic {
            get {
                return delegate(float progress) {
                    if (progress < 0.5F) {
                        return 4 * Math.Pow(progress, 3);
                    } else {
                        return 1 - Math.Pow(-2 * progress + 2, 3) / 2;
                    }
                };
            }
        }

        public static EasingFunction EaseInQuart {
            get {
                return delegate(float progress) {
                    return Math.Pow(progress, 4);
                };
            }
        }

        public static EasingFunction EaseOutQuart {
            get {
                return delegate(float progress) {
                    return 1 - Math.Pow(1 - progress, 4);
                };
            }
        }

        public static EasingFunction EaseInOutQuart {
            get {
                return delegate(float progress) {
                    if (progress < 0.5F) {
                        return 8 * Math.Pow(progress, 4);
                    } else {
                        return 1 - Math.Pow(-2 * progress + 2, 4) / 2;
                    }
                };
            }
        }

        public static EasingFunction EaseInOutQuartWithStep7 {
            get {
                return delegate(float progress) {
                    if (progress > 0.9F) {
                        return 3 * progress - 2;
                    } else if (progress < 0.45F) {
                        return 5.6F * Math.Pow(progress * 1.1F, 4);
                    } else {
                        return 0.7F - Math.Pow(-2.2F * progress + 2, 4) * 0.35F;
                    }
                };
            }
        }

        public static EasingFunction EaseInOutQuartWithStep4 {
            get {
                return delegate(float progress) {
                    if (progress > 0.9F) {
                        return 6 * progress - 5;
                    } else if (progress < 0.45F) {
                        return 3.2F * Math.Pow(progress * 1.1F, 4);
                    } else {
                        return 0.4F - Math.Pow(-2.2F * progress + 2, 4) * 0.2F;
                    }
                };
            }
        }
    }


    internal class IndicatorColor {
        public IndicatorColor(Color colorOff, Color colorOn, int code) {
            this.ColorOff = colorOff;
            this.ColorOn = colorOn;
            this.Code = code;
        }

        public int Code = -1;
        public Color ColorOff;
        public Color ColorOn;
        public Rectangle Rectangle = Rectangle.Empty;
        public bool Pressed = false;
        public bool LastUnpressed = false;
        public bool MouseOver = false;

        public static IndicatorColor Red {
            get { return new IndicatorColor(Color.FromArgb(255, 204, 204), Color.FromArgb(255, 102, 51), 0x01); }
        }
        public static IndicatorColor Green {
            get { return new IndicatorColor(Color.FromArgb(204, 255, 102), Color.FromArgb(51, 204, 51), 0x02); }
        }
        public static IndicatorColor Yellow {
            get { return new IndicatorColor(Color.FromArgb(255, 255, 51), Color.FromArgb(205, 173, 0), 0x03); }
        }
    }
}
