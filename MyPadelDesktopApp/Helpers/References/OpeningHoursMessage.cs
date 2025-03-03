using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Helpers.References
{
    public class OpeningHoursMessage : ValueChangedMessage<string>
    {
        public OpeningHoursMessage(string value) : base(value) { }
    }
}
