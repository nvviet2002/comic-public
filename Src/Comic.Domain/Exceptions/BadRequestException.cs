using Comic.Domain.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException()
        {

        }

        public BadRequestException(string message) : base($"{AppSetting.AppName}: {message}")
        {

        }

        public BadRequestException(string message, Exception innerException) : base($"{AppSetting.AppName}: {message}", innerException)
        {

        }

    }
}
