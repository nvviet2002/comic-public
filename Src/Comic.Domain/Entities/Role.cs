using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comic.Domain.Common;
using Comic.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Comic.Domain.Entities
{
    [Table("Roles")]
    public class Role : IdentityRole, IBaseEntity
    {
        public Role(): base()
        {

        }
        public Role(string roleName) : base(roleName)
        {

        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActived { get; set; } = true;
    }
}
