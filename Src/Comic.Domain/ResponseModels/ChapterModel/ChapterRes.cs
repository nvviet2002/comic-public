using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.ResponseModels.StoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.ResponseModels.ChapterModel
{
    public class ChapterRes
    {
        public string Id { get; set; }
        public string StoryId { get; set; }
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
        //public StoryRes? Story { get; set; }
        public ICollection<string>? Images { get; set; }


    }
}
