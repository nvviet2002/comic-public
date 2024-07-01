﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Comic.Domain.Common;

namespace Comic.Domain.Entities
{
    [Table("Follows")]
    public class Follow: BaseEntity
    {
        public string StoryId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [ForeignKey("StoryId")]
        public virtual Story? Story { get; set; }
    }
}
