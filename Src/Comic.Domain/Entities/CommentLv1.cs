using Comic.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Entities
{
    public class CommentLv1: BaseEntity
    {
        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? ChapterId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("ChapterId")]
        public Chapter? Chapter { get; set; }
        public ICollection<CommentLv2>?  CommentLv2s { get; set; }


    }
}
