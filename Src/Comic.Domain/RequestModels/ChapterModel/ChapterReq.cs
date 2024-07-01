using Comic.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.RequestModels.ChapterModel
{
    public class ChapterReq
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(250, ErrorMessage = "Tên tối đa {1} kí tự")]
        public string? Name { get; set; }
        [MaxLength(250, ErrorMessage = "Tiêu đề tối đa {1} kí tự")]
        public string? Title { get; set; }
        [MaxLength(250, ErrorMessage = "Các từ khóa SEO tối đa {1} kí tự")]
        public string? MetaKeyword { get; set; }
        [MaxLength(1000, ErrorMessage = "Mô tả SEO tối đa {1} kí tự")]
        public string? MetaDescription { get; set; }
        [Range(0,2100000000,ErrorMessage ="Lượt xem chỉ từ {0} -> {1}")]
        public int? View { get; set; }
        [Range(0, 2100000000, ErrorMessage = "Chỉ mục chỉ từ {0} -> {1}")]
        public int? Index { get; set; }
        [Required(ErrorMessage = "Tình trạng không được để trống")]
        public ChapterStatus? Status { get; set; }
        public bool? HotFlag { get; set; }
        public bool? IsActived { get; set; }
        public IList<IFormFile>? Images { get; set; }
    }
}
