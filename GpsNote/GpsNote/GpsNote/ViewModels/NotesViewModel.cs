using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNote.ViewModels
{
    public class NotesViewModel : ViewModelBase
    {
        public NotesViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
