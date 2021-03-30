using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNote.Services.Settings
{
    public interface ISettings
    {
        int LoggedUser { get; set; }
    }
}