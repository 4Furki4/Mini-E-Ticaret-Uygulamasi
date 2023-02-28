using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException() : base("E-posta veya şifre hatalı!")
        {

        }

        public LoginFailedException(string? message) : base(message)
        {
        }
    }
}
