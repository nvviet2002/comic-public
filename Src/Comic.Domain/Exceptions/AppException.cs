using Comic.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Exceptions
{
    public class AppException: Exception
    {
        public AppException()
        {

        }

        public AppException(string message) : base($"{AppSetting.AppName}: {message}")
        {

        }

        public AppException(string message, Exception innerException) : base($"{AppSetting.AppName}: {message}", innerException)
        {

        }
    }
}
