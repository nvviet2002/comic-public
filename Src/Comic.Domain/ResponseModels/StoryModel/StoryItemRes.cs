using Comic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.ResponseModels.StoryModel
{
    public class StoryItemRes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OtherName { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int View { get; set; }
        public float Rate { get; set; }
        public int RateTotal { get; set; }
        public int Follow { get; set; }
        public int Comment { get; set; }
        public StoryStatus Status { get; set; }
        public bool HotFlag { get; set; }
        public string Slug { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActived { get; set; }
        public ICollection<string> CategoryNames { get; set; }
    }
}
