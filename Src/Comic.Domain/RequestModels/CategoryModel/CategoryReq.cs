using Comic.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.RequestModels.CategoryModel
{
    public class CategoryReq
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(250,ErrorMessage ="Tên tối đa {1} kí tự")]
        public string? Name { get; set; }
        [MaxLength(1000, ErrorMessage ="Mô tả tối đa {1} kí tự")]
        public string? Description { get; set; }
        [MaxLength(250, ErrorMessage = "Các từ khóa SEO tối đa {1} kí tự")]
        public string? MetaKeyword { get; set; }
        [MaxLength(1000, ErrorMessage = "Mô tả SEO tối đa {1} kí tự")]
        public string? MetaDescription { get; set; }
        [Required(ErrorMessage = "Phân loại không được để trống")]
        public CategoryType? Type { get; set; }
        public bool? IsActived { get; set; }
    }
}
