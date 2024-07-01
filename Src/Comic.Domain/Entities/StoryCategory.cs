using Comic.Domain.Common;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Entities
{

    [Table("StoryCategories")]
    public class StoryCategory: BaseEntity
    {
        public string StoryId { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [ForeignKey("StoryId")]
        public Story? Story { get; set; }
    }
}
