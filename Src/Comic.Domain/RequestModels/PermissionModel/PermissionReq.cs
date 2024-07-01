using System.ComponentModel.DataAnnotations;

namespace Comic.Domain.RequestModels.PermissionModel
{
    public class PermissionReq
    {
        [Required(ErrorMessage = "Tên quyền không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên quyền dài tối đa 100 kí tự")]
        public string Name { get; set; }
    }
}
