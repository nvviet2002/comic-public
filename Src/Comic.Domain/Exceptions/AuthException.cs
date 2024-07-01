using Comic.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Exceptions
{
    public class AuthException: Exception
    {
        public AuthException()
        {

        }

        public AuthException(string message) : base($"{AppSetting.AppName}: {message}")
        {

        }

        public AuthException(string message, Exception innerException) : base($"{AppSetting.AppName}: {message}", innerException)
        {

        }
    }
}
