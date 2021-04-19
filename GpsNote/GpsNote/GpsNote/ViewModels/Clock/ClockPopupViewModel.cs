using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace GpsNote.ViewModels.Clock
{
    public class ClockPopupViewModel
    {
        public ICommand BackgroundClickedCommand => new Command(BackgroundClickedCommandExecute);

        private void BackgroundClickedCommandExecute(object parameter)
        {
            var label = (Label)parameter;
            label.Text = "Great, it works!";
        }
    }
}