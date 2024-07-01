using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Comic.Application.Common
{
    public class SeoHelper
    {
        public static string GenerateSlug(string text)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string slug = text.Normalize(NormalizationForm.FormD).Trim().ToLower();

            slug = regex.Replace(slug, String.Empty)
              .Replace('\u0111', 'd').Replace('\u0110', 'D')
              .Replace(",", "-").Replace(".", "-").Replace("!", "")
              .Replace("(", "").Replace(")", "").Replace(";", "-")
              .Replace("/", "-").Replace("%", "ptram").Replace("&", "va")
              .Replace("?", "").Replace('"', '-').Replace(' ', '-');
            return slug;
        }
    }
}
