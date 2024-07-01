using Comic.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Exceptions
{
    public class ValidateException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }
        public ValidateException()
        {

        }

        public ValidateException(string message) : base($"{AppSetting.AppName}: {message}")
        {

        }

        public ValidateException(string message, Exception innerException) : base($"{AppSetting.AppName}: {message}", innerException)
        {

        }

        public ValidateException(string message, Exception innerException, Dictionary<string, string> errors)
            : base($"{AppSetting.AppName}: {message}", innerException)
        {
            Errors = errors;
        }

        public ValidateException(string message, Dictionary<string, string> errors) : base($"{AppSetting.AppName}: {message}")
        {
            Errors = errors;
        }
    }
}
