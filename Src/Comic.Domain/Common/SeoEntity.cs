using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Common
{
    public class SeoEntity : BaseEntity, ISeo
    {
        [MaxLength(250)]
        public string Slug { get; set; } = string.Empty;
        [MaxLength(250)]
        public string MetaKeyword { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string MetaDescription { get; set; } = string.Empty;
    }
}
