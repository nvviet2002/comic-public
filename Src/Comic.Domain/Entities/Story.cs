using Comic.Domain.Common;
using Comic.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Comic.Domain.Entities
{
    [Table("Stories")]
    public class Story: SeoEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(250)]
        public string OtherName { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Avatar { get; set; } = string.Empty;
        public int View { get; set; } = 0;
        public float Rate { get; set; } = 0.0f;
        public int RateTotal { get; set; } = 0;
        public int Follow { get; set; } = 0;
        public int Comment { get; set; } = 0;
        public StoryStatus Status { get; set; } = StoryStatus.New;
        public bool HotFlag { get; set; } = false;
        public ICollection<StoryCategory>? StoryCategories { get; set; }
        public ICollection<Chapter>? Chapters { get; set; }
        public ICollection<Follow>? Follows { get; set; }
    }
}
