using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comic.Domain.Common;

namespace Comic.Domain.Entities
{
    [Table("ChapterImages")]
    public class ChapterImage: BaseEntity
    {
        public float Index { get; set; } = 0.0f;
        [MaxLength(250)]
        public string Path { get; set; } = string.Empty;
        public string? ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }
    }
}
