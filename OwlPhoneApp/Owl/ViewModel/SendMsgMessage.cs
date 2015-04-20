using System;
using System.Linq;

namespace Owl.ViewModel
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
