using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Prism.Ioc;
using GpsNote.Services.Color;


namespace GpsNote.Controls
{
    public class CustomCanvas : SKCanvasView
    {
        private IColorService _colorService;
        private readonly SKPaint _grayFillPaint;
        private readonly SKPaint _lightStrokePaint;
        private readonly SKPaint _darkStrokePaint;
        private readonly SKPaint _digitsFillPaint;
        private static DateTime _dateTime;
        private static bool _timerAlive;
        private readonly SKColor _backgroundColor;
        private readonly SKColor _faceColor;
        private readonly SKColor _digitColor;


        public CustomCanvas()
        {
            _lightStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2
            };

            _darkStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2
            };

            _dateTime = default(DateTime);
            _timerAlive = false;

            SetColorService();
            _lightStrokePaint.Color = _colorService.GetCurrentLightColor();
            _darkStrokePaint.Color = _colorService.GetCurrentDarkColor();

            if (_colorService.IsDarkTheme())
            {
                _backgroundColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["blackClock"] as string).ToSKColor();

                _faceColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["faceDarkClock"] as string).ToSKColor();

                _digitColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["systemWhiteClock"] as string).ToSKColor();
            }
            else
            {
                _backgroundColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["systemWhiteClock"] as string).ToSKColor();

                _faceColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["grayFillClock"] as string).ToSKColor();

                _digitColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["blackClock"] as string).ToSKColor();
            }

            _grayFillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = _faceColor
            };

            _digitsFillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = _digitColor
            };

            _timerAlive = true;
            this.PaintSurface += OnCustomCanvas_PaintSurface;

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                this.InvalidateSurface();
                return _timerAlive;
            });
        }


        #region -- Public properties --

        public static readonly BindableProperty TimeZoneLocalTimeProperty =
            BindableProperty.Create(nameof(TimeZoneLocalTime),
                                    typeof(DateTime),
                                    typeof(CustomCanvas),
                                    defaultValue: default(DateTime),
                                    propertyChanged: TimeZoneLocalTimePropertyChanged);


        public DateTime TimeZoneLocalTime
        {
            get => (DateTime)GetValue(TimeZoneLocalTimeProperty);
            set => SetValue(TimeZoneLocalTimeProperty, value);
        }


        public static readonly BindableProperty TimerAliveProperty =
            BindableProperty.Create(nameof(TimerAlive),
                                    typeof(bool),
                                    typeof(CustomCanvas),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    defaultValue: false,
                                    propertyChanged: TimerAlivePropertyChanged);


        public bool TimerAlive
        {
            get => (bool)GetValue(TimerAliveProperty);
            set => SetValue(TimerAliveProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private void OnCustomCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(_backgroundColor);

            int width = e.Info.Width;
            int height = e.Info.Height;

            float radius = (width / 10f) < (height / 10f) ? width / 11f : height / 11f;

            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 200f);


            canvas.DrawCircle(0, 0, radius, _grayFillPaint);
            
            canvas.DrawCircle(0, 0, radius + 1, _lightStrokePaint);
            canvas.DrawCircle(0, 0, 2, _darkStrokePaint);

            _lightStrokePaint.StrokeWidth = 2;

            for (int angle = 0; angle < 360; angle += 90)
            {
                canvas.DrawLine(2, radius, 2, radius - 7, _lightStrokePaint);
                canvas.RotateDegrees(90);
            }


            canvas.DrawText("12", -8, -radius + 20, _digitsFillPaint);
            canvas.DrawText("6", -2, radius - 12, _digitsFillPaint);
            canvas.DrawText("9", -radius + 10, 5, _digitsFillPaint);
            canvas.DrawText("3", radius - 15, 3, _digitsFillPaint);

            // hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * _dateTime.Hour + _dateTime.Minute / 2f);
            _darkStrokePaint.StrokeWidth = 3;
            canvas.DrawLine(0, -2, 0, -radius / 2, _darkStrokePaint);
            canvas.Restore();

            // minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * _dateTime.Minute + _dateTime.Second / 10f);
            _darkStrokePaint.StrokeWidth = 2;
            canvas.DrawLine(0, -2, 0, -radius * 0.75f, _darkStrokePaint);
            canvas.Restore();

            // second hand
            canvas.Save();
            float seconds = _dateTime.Second + _dateTime.Millisecond / 1000f;
            canvas.RotateDegrees(6 * seconds);
            _lightStrokePaint.StrokeWidth = 1;
            canvas.DrawLine(0, -3, 0, -radius * 0.9f, _lightStrokePaint);
            canvas.Restore();
        }


        private static void TimeZoneLocalTimePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            _dateTime = (DateTime)newValue;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _dateTime = _dateTime.AddSeconds(1);
                return _timerAlive;
            });
        }

        private static void TimerAlivePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomCanvas canvas = bindable as CustomCanvas;

            if(canvas != null)
            {
                _timerAlive = (bool)newValue;
            }
        }


        private void SetColorService()
        {
            if(_colorService == null)
            {
                App app = (App)Application.Current;
                _colorService = app.Container.Resolve<IColorService>();
            }
        }

        #endregion
    }
}