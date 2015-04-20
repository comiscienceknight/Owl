using Owl.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owl.ViewModel
{
    public class NavigateToChatMessage
    {
        public static object Token = new object();
        public ChatEntry ChatEntry { get; set; }

        public NavigateToChatMessage()
            : this(null)
        {

        }

        public NavigateToChatMessage(ChatEntry chatEntry)
        {
            ChatEntry = chatEntry;
        }
    }
}
