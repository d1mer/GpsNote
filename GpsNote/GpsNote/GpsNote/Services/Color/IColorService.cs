using GpsNote.Enums;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNote.Services.Color
{
    public interface IColorService
    {
        SKColor GetCurrentLightColor();

        SKColor GetCurrentDarkColor();

        void SaveCurrentColor(Colors color);

        int GetCurrentColor();
    }
}
