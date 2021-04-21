using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Prism.Ioc;
using GpsNote.Services.Color;
using Prism;

namespace GpsNote.Controls
{
    public class CustomCanvas : SKCanvasView
    {
        private IColorService _colorService;

        private readonly SKPaint grayFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = Xamarin.Forms.Color.FromHex(Constants.GRAY_FILL_CLOCK_COLOR).ToSKColor()
        };

        private readonly SKPaint lightStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        private readonly SKPaint darkStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        private readonly SKPaint digitsFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

        private static DateTime _dateTime = default(DateTime);
        private static bool _timerAlive = false;


        public CustomCanvas()
        {
            SetColorService();
            lightStrokePaint.Color = _colorService.GetCurrentLightColor();
            darkStrokePaint.Color = _colorService.GetCurrentDarkColor();
            
            _timerAlive = true;
            this.PaintSurface += CustomCanvas_PaintSurface;

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
        private void CustomCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.White);

            int width = e.Info.Width;
            int height = e.Info.Height;

            float radius = (width / 10f) < (height / 10f) ? width / 10f : height / 10f;

            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 200f);


            canvas.DrawCircle(0, 0, radius, grayFillPaint);
            canvas.DrawCircle(0, 0, radius + 1, lightStrokePaint);
            canvas.DrawCircle(0, 0, 2, darkStrokePaint);

            lightStrokePaint.StrokeWidth = 2;

            for (int angle = 0; angle < 360; angle += 90)
            {
                canvas.DrawLine(2, radius, 2, radius - 7, lightStrokePaint);
                canvas.RotateDegrees(90);
            }


            canvas.DrawText("12", -8, -radius + 20, digitsFillPaint);
            canvas.DrawText("6", -2, radius - 12, digitsFillPaint);
            canvas.DrawText("9", -radius + 10, 5, digitsFillPaint);
            canvas.DrawText("3", radius - 15, 3, digitsFillPaint);

            // hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * _dateTime.Hour + _dateTime.Minute / 2f);
            darkStrokePaint.StrokeWidth = 3;
            canvas.DrawLine(0, -2, 0, -radius / 2, darkStrokePaint);
            canvas.Restore();

            // minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * _dateTime.Minute + _dateTime.Second / 10f);
            darkStrokePaint.StrokeWidth = 2;
            canvas.DrawLine(0, -2, 0, -radius * 0.75f, darkStrokePaint);
            canvas.Restore();

            // second hand
            canvas.Save();
            float seconds = _dateTime.Second + _dateTime.Millisecond / 1000f;
            canvas.RotateDegrees(6 * seconds);
            lightStrokePaint.StrokeWidth = 1;
            canvas.DrawLine(0, -3, 0, -radius * 0.9f, lightStrokePaint);
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