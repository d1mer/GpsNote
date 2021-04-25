using Prism.Mvvm;

namespace GpsNote.ViewModels.ExtentedViewModels
{
    public class PinViewModel : BindableBase
    {
        #region -- Publics --

        private int id;
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private double latitude;
        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        private double longitude;
        public double Longtitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        private string label;
        public string Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }

        private string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private int owner;
        public int Owner
        {
            get => owner;
            set => SetProperty(ref owner, value);
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        private string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set => SetProperty(ref imagePath, value);
        }

        #endregion
    }
}