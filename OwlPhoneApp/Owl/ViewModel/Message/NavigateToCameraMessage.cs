using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlWindowsPhoneApp.ViewModel.Message
{
    public class NavigateToCameraMessage
    {
        public static object Token = new object();
        public int ProfileNumber { get; set; }

        public NavigateToCameraMessage()
            : this(0)
        {

        }

        public NavigateToCameraMessage(int profileNumber)
        {
            ProfileNumber = profileNumber;
        }
    }
}
