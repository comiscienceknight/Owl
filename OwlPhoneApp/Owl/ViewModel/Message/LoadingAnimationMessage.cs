using System;
using System.Linq;

namespace Owl.ViewModel
{
    public class LoadingAnimationMessage
    {
        public static object PostToken = new object();
        public static object ChatToken = new object();
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
