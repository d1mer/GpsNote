using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNote.Helpers
{
    public class DoubleOutFormat
    {
        public static string Format(double digit) => string.Format("{0:###.########}", digit);
    }
}