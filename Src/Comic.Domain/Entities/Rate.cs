using Comic.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Entities
{
    [Table("Rates")]
    public class Rate: BaseEntity
    {
        public float Star { get; set; } = 0.0f;
        public string StoryId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [ForeignKey("StoryId")]
        public virtual Story? Story { get; set; }
    }
}
