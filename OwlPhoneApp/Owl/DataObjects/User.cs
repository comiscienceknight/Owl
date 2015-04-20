using System;
using System.Linq;

namespace Owl.DataObjects
{
    public class User
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Sexe { get; set; }
        public int Popularity { get; set; }
        public DateTimeOffset Birthday { get; set; }
    }
}
