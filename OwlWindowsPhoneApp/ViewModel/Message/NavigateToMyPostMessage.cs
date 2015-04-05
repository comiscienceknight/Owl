using OwlWindowsPhoneApp.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace OwlWindowsPhoneApp.ViewModel
{
    public class NavigateToMyPostMessage
    {
        public static object Token = new object();
        public BitmapImage Image { get; set; }

        public NavigateToMyPostMessage()
            : this(null)
        {

        }

        public NavigateToMyPostMessage(BitmapImage image)
        {
            Image = image;
        }
    }
}
