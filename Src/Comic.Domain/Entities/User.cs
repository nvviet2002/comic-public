using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Comic.Domain.Interfaces;

namespace Comic.Domain.Entities
{
    [Table("Users")]
    public class User: IdentityUser, IBaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { set; get; } = string.Empty;

        public DateTime? Birthday { set; get; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(13,1)")]
        public decimal Point { get; set; } = 0.0m;

        [MaxLength(300)]
        public string? Avatar { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActived { get; set; } = true;
    }
}
