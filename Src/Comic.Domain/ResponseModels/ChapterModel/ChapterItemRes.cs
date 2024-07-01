using Comic.Domain.Entities;
using Comic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.ResponseModels.ChapterModel
{
    public class ChapterItemRes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int View { get; set; }
        public int Index { get; set; }
        public int Comment { get; set; }
        public ChapterStatus Status { get; set; }
        public bool HotFlag { get; set; }
        public string Slug { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActived { get; set; }
    }
}
