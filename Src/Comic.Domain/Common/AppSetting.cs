using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Common
{
    public static class AppSetting
    {
        public static string? DefaultConnection { get; set; }
        public static string? AppName { get; set; }
        public static string? JWTValidAudience { get; set; }
        public static string? JWTValidIssuer { get; set; }
        public static string? JWTSecretKey { get; set; }
        public static double JWTATExpireTime { get; set; }
        public static double JWTRTExpireTime { get; set; }
        public static string? FileRootUrl { get; set; }
        public static string? AwsAccessKey { get; set; }
        public static string? AwsSecretKey { get; set; }
        public static string? AwsBucketName { get; set; }
        public static string? AwsServiceUrl { get; set; }


    }
}
