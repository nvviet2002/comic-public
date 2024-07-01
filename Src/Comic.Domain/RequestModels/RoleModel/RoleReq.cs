using System.ComponentModel.DataAnnotations;

namespace Comic.Domain.RequestModels.RoleModel
{
    public class RoleReq
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên dài tối đa 100 kí tự")]
        public string Name { get; set; }
        public ICollection<string>? PermissionIds { get; set; }
    }
}
