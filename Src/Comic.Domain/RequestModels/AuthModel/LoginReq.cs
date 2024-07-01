using System.ComponentModel.DataAnnotations;

namespace Comic.Domain.RequestModels.AuthModel
{
    public class LoginReq
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không quá 100 ký tự")]
        public string Password { get; set; }
    }
}
