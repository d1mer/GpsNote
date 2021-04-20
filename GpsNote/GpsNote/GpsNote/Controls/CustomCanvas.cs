using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

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

        private SKPaint centralCircleStrokePaint = new SKPaint
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

        public CustomCanvas()
        {
            this.PaintSurface += CustomCanvas_PaintSurface;
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

            canvas.DrawCircle(0, 0, 75, grayFillPaint);
            canvas.DrawCircle(0, 0, 76, lightBlueStrokePaint);
            canvas.DrawCircle(0, 0, 2, centralCircleStrokePaint);

            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            whiteStrokePaint.StrokeWidth = 15;
            canvas.DrawLine(0, 0, 0, -50, whiteStrokePaint);
            canvas.Restore();
        }
    }
}