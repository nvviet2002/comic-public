using Comic.Application.Services;
using Comic.Domain.Entities;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.StoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface IMapperService
    {
        StoryItemRes MapStoryItemRes(Story story);
        StoryRes MapStoryRes(Story story);

        ChapterItemRes MapChapterItemRes(Chapter chapter);
        ChapterRes MapChapterRes(Chapter chapter);
    }
}
