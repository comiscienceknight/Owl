using System;
using System.Linq;

namespace OwlWindowsPhoneApp.ViewModel
{
    public class LoadingAnimationMessage
    {
        public static object Token = new object();
        public bool IfLoading { get; set; }

        public LoadingAnimationMessage()
            :this(false)
        {

        }

        public LoadingAnimationMessage(bool ifLoading)
        {
            IfLoading = ifLoading;
        }
    }
}
