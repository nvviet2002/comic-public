using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comic.Domain.Common;

namespace Comic.Domain.Entities
{
    public class CommentLv2: BaseEntity
    {
        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? CommentLv1Id { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("CommentLv1Id")]
        public CommentLv1? CommentLv1 { get; set; }
    }
}
