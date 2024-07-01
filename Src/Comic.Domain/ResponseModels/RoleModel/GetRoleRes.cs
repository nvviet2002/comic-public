using Comic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.ResponseModels.RoleModel
{
    public class GetRoleRes
    {
        public Entities.Role Role { get; set; }
        public ICollection<Permission>? Permissions { get; set; }
    }
}
