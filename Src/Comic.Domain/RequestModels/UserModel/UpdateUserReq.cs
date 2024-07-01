using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.RequestModels.UserModel
{
    public class UpdateUserReq
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        public string? Email { get; set; }
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Mật khẩu chỉ từ {2} -> {1} ký tự")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? Point { get; set; }
        public IFormFile? Avatar { get; set; }
        public bool? IsActived { get; set; }
        public ICollection<string>? RoleIds { get; set; }
    }
}
