using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GpsNote.Controls
{
    public class CustomCanvas : SKCanvasView
    {
        private SKPaint grayFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = Xamarin.Forms.Color.FromHex("#F1F3FD").ToSKColor()
        };

        private SKPaint lightBlueStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Xamarin.Forms.Color.FromHex("#C7CDF5").ToSKColor(),
            StrokeWidth = 2
        };

        private SKPaint darkBlueStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Xamarin.Forms.Color.FromHex("#596EFB").ToSKColor(),
            StrokeWidth = 2
        };

        private SKPaint hourStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Xamarin.Forms.Color.FromHex("#596EFB").ToSKColor(),
            StrokeWidth = 3
        };

        private SKPaint digitsFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

        public CustomCanvas()
        {
            this.PaintSurface += CustomCanvas_PaintSurface;

            Device.StartTimer(TimeSpan.FromSeconds(1f / 600), () =>
            {
                this.InvalidateSurface();
                return true;
            });
        }

        private void CustomCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.White);

            int width = e.Info.Width;
            int height = e.Info.Height;

            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 200f);

            DateTime dateTime = DateTime.Now;

            canvas.DrawCircle(0, 0, height / 10f, grayFillPaint);
            canvas.DrawCircle(0, 0, height / 10f + 1, lightBlueStrokePaint);
            canvas.DrawCircle(0, 0, 2, darkBlueStrokePaint);

            lightBlueStrokePaint.StrokeWidth = 2;
            int time = 6;

            for (int angle = 0; angle < 360; angle += 90)
            {
                canvas.DrawLine(2, height / 10f, 2, height / 10f - 7, lightBlueStrokePaint);

                if(time == 12)
                {
                    canvas.Save();
                    canvas.RotateDegrees(-180);
                    canvas.DrawText(time.ToString(), -4, height / 10f - 10, digitsFillPaint);
                    canvas.Restore();
                }
                else
                {
                    canvas.DrawText(time.ToString(), -1, height / 10f - 10, digitsFillPaint);
                }

                if(time == 12)
                {
                    time = 3;
                }
                else
                {
                    time += 3;
                }

                canvas.RotateDegrees(90);
            }

            // hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            darkBlueStrokePaint.StrokeWidth = 3;
            canvas.DrawLine(0, -2, 0, -40, darkBlueStrokePaint);
            canvas.Restore();

            // minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            darkBlueStrokePaint.StrokeWidth = 2;
            canvas.DrawLine(0, -2, 0, -50, darkBlueStrokePaint);
            canvas.Restore();

            // second hand
            canvas.Save();
            float seconds = dateTime.Second + dateTime.Millisecond / 1000f;
            canvas.RotateDegrees(6 * seconds);
            lightBlueStrokePaint.StrokeWidth = 1;
            canvas.DrawLine(0, -3, 0, -65, lightBlueStrokePaint);
            canvas.Restore();
        }
    }
}