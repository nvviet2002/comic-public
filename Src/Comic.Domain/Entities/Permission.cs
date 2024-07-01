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
    [Table("Permissions")]
    public class Permission: BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? NormalizedName { get; set; } = string.Empty;
        
    }
}
