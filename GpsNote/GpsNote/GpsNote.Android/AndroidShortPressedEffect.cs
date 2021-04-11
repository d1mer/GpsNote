using Android.App;
using GpsNote.Droid;
using GpsNote.Effects;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("GpsNote")]
[assembly: ExportEffect(typeof(AndroidShortPressedEffect), "ShortPressedEffect")]
namespace GpsNote.Droid
{
    [Application(Theme = "@style/MainTheme")]
    class AndroidShortPressedEffect : PlatformEffect
    {
        private bool _attached;

        public AndroidShortPressedEffect() { }

        public static void Initialize() { }


        protected override void OnAttached()
        {
            if (!_attached)
            {
                if(Control != null)
                {
                    Control.Clickable = true;
                    Control.Click += Control_Click;
                }
                else
                {
                    Container.Clickable = true;
                    Container.Click += Control_Click;
                }
                _attached = true;
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Invoking click command");
            System.Windows.Input.ICommand command = ShortPressedEffect.GetCommand(Element);
            command?.Execute(ShortPressedEffect.GetCommandParameter(Element));
        }


        protected override void OnDetached()
        {
            if (_attached)
            {
                if (Control != null)
                {
                    Control.Clickable = true;
                    Control.Click -= Control_Click;
                }
                else
                {
                    Container.Clickable = true;
                    Container.Click -= Control_Click;
                }
                _attached = false;
            }
        }
    }
}