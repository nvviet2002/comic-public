using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.StoryModel;
using System.Globalization;

namespace Comic.WebClient.Common
{
    public class RazorHelper
    {
        //public static DateTime UtcToLocalTime(DateTime utcTime)
        //{
        //    return utcTime.ToLocalTime();
        //}

        public static string UtcToComicString (DateTime utcTime)
        {
            var timeSpan = DateTime.UtcNow - utcTime;
            if(timeSpan.Days > 365)
            {
                return utcTime.ToLocalTime().ToString("dd/MM/yyyy");
            }
            else if(timeSpan.Days > 30)
            {
                return $"{Math.Floor((decimal)timeSpan.Days/30)} tháng trước";
            }
            else if (timeSpan.Days >= 1)
            {
                return $"{timeSpan.Days} ngày trước";
            }
            else if (timeSpan.Hours >= 1)
            {
                return $"{timeSpan.Hours} giờ trước";
            }
            else if (timeSpan.Minutes >= 1)
            {
                return $"{timeSpan.Minutes} phút trước";
            }
            else
            {
                return $"{timeSpan.Seconds} giây trước";
            }
        }


        public static string GeneratePagination(PaginateRes<StoryRes> sdsd)
        {
            return "";
        }
    }
}
