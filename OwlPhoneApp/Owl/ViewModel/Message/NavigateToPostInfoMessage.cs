using Owl.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owl.ViewModel
{
    public class NavigateToPostInfoMessage
    {
        public static object Token = new object();
        public Post Post {get;set;}

        public NavigateToPostInfoMessage()
            :this(null)
        {

        }

        public NavigateToPostInfoMessage(Post post)
        {
            Post = post;
        }
    }
}
