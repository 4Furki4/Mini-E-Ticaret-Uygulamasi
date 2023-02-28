using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.ViewModels.Identity
{
    public class LoginViewModel
    {
        public string UserNameOrEmail{ get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
