using Comic.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message) : base($"{AppSetting.AppName}: {message}")
        {

        }

        public NotFoundException(string message, Exception innerException) : base($"{AppSetting.AppName}: {message}", innerException)
        {

        }
    }
}
