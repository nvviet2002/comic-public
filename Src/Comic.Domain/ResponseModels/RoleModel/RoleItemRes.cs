using Comic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.ResponseModels.RoleModel
{
    public class RoleItemRes
    {
        public Role Role { get; set; }
        public int UserCount { get; set; }
    }
}
