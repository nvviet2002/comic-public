using Comic.Domain.Common;
using Comic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Entities
{
    [Table("Categories")]
    public class Category: SeoEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty ;
        public CategoryType Type { get; set; } = CategoryType.Category;
        public ICollection<StoryCategory>? StoryCategories { get; set; }
    }
}
