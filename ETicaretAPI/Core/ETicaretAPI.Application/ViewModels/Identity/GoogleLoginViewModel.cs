using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.ViewModels.Identity
{
    public class GoogleLoginViewModel
    {
        public string id { get; set; }
        public string idToken { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string photoUrl { get; set; }
        public string provider { get; set; }
    }
}
