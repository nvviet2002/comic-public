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
    [Table("Chapters")]
    public class Chapter: SeoEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(250)]
        public string Title { get; set; } = string.Empty;
        public int View { get; set; } = 0;
        public int Comment { get; set; } = 0;
        public int Index { get; set; } = 0;
        public ChapterStatus Status { get; set; } = ChapterStatus.Raw;
        public bool HotFlag { get; set; } = false;
        public string? StoryId { get; set; }
        [ForeignKey("StoryId")]
        public Story? Story { get; set; }
        public ICollection<ChapterImage>? ChapterImages { get; set; }
        public ICollection<CommentLv1>? CommentLv1s { get; set; }
    }
}
