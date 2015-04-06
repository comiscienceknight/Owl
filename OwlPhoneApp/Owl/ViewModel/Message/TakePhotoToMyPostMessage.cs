using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace OwlWindowsPhoneApp.ViewModel.Message
{
    public class TakePhotoToMyPostMessage
    {
        public static object Token = new object();
        public static object ChatToken = new object();
        public RenderTargetBitmap BitMap { get; set; }
        public int ProfileNumber { get; set; }

        public TakePhotoToMyPostMessage()
            :this(null)
        {

        }

        public TakePhotoToMyPostMessage(RenderTargetBitmap bitMap)
        {
            BitMap = bitMap;
        }
    }
}
