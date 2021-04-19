using GpsNote.ViewModels.Clock;

namespace GpsNote.Views.Clock
{
    public partial class ClockPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ClockPopupPage()
        {
            InitializeComponent();
            BindingContext = new ClockPopupViewModel();
        }
    }
}