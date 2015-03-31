using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient client = new WebClient())
            {
                client.UploadFile("http://localhost:51117/UploadPicture", "wellington4.jpg");
            }
        }
    }
}
