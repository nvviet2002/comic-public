using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.Common
{
    public class DataHelper
    {
        public static DateTime UnixTimeToDateTime(long unix)
        {
            var rootTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return rootTime.AddSeconds(unix).ToUniversalTime();
        }
    }
}
