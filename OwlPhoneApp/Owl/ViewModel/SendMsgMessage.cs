using System;
using System.Linq;

namespace OwlWindowsPhoneApp.ViewModel
{
    public class SendMsgMessage
    {
        public static object Token = new object();
        public bool IfLoading { get; set; }

        public SendMsgMessage()
            :this(false)
        {

        }

        public SendMsgMessage(bool ifLoading)
        {
            IfLoading = ifLoading;
        }
    }
}
