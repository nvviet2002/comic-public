using Comic.Domain.Enums;
using Comic.Domain.RequestModels.PaginateModel;

namespace Comic.Domain.RequestModels.StoryModel
{
    public class StorySearchReq
    {
        public PaginateReq PaginateReq { get; set; }
        public string? CategoryId { get; set; }
        public StoryStatus? Status { get; set; }
        public SortOption? SortOption { get; set; } = Comic.Domain.Enums.SortOption.UpdatedAt;
    }
}
