using Comic.Application.IServices;
using Comic.Domain.Entities;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.StoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.Services
{
    public class MapperService: IMapperService
    {
        private readonly IFileService _fileService;

        public MapperService(IFileService fileService)
        {
            _fileService = fileService;
        }

        //story
        public StoryItemRes MapStoryItemRes(Story story)
        {
            var categoryNames = new List<string>();
            foreach (var storyCate in story.StoryCategories)
            {
                categoryNames.Add(storyCate.Category.Name);
            }
            var storyItemRes = new StoryItemRes()
            {
                Id = story.Id,
                Name = story.Name,
                OtherName = story.OtherName,
                Description = story.Description,
                MetaDescription = story.MetaDescription,
                MetaKeyword = story.MetaKeyword,
                Rate = story.Rate,
                Comment = story.Comment,
                Follow = story.Follow,
                View = story.View,
                RateTotal = story.RateTotal,
                HotFlag = story.HotFlag,
                IsActived = story.IsActived,
                Slug = story.Slug,
                Status = story.Status,
                Avatar = _fileService.GetFileUrl(story.Avatar),
                CreatedAt = story.CreatedAt,
                UpdatedAt = story.UpdatedAt,
                CategoryNames = categoryNames
            };
            return storyItemRes;
        }
        public StoryRes MapStoryRes(Story story)
        {
            var newStoryRes = new StoryRes()
            {
                Id = story.Id,
                Name = story.Name,
                OtherName = story.OtherName,
                Description = story.Description,
                MetaDescription = story.MetaDescription,
                MetaKeyword = story.MetaKeyword,
                Avatar = _fileService.GetFileUrl(story.Avatar),
                View = story.View,
                Comment = story.Comment,
                Follow = story.Follow,
                Rate = story.Rate,
                Slug = story.Slug,
                RateTotal = story.RateTotal,
                Status = story.Status,
                HotFlag = story.HotFlag,
                IsActived = story.IsActived,
                CreatedAt = story.CreatedAt,
                UpdatedAt = story.UpdatedAt,
                Categories = new List<Category>(),
                Chapters = new List<ChapterRes>(),
            };
            
            if (story.StoryCategories != null)
            {
                foreach (var storyCate in story.StoryCategories)
                {
                    storyCate.Category.StoryCategories = null;
                    newStoryRes.Categories.Add(storyCate.Category);
                }
            }
            if (story.Chapters != null)
            {
                foreach (var chapter in story.Chapters)
                {
                    var newChapterRes = MapChapterRes(chapter);
                    newStoryRes.Chapters.Add(newChapterRes);
                }
            }
           
            return newStoryRes;
        }

        //chapter

        public ChapterItemRes MapChapterItemRes(Chapter chapter)
        {
            var chapterItemRes = new ChapterItemRes()
            {
                Id = chapter.Id,
                Name = chapter.Name,
                Title = chapter.Title,
                MetaDescription = chapter.MetaDescription,
                MetaKeyword = chapter.MetaKeyword,
                Comment = chapter.Comment,
                View = chapter.View,
                Index = chapter.Index,
                HotFlag = chapter.HotFlag,
                IsActived = chapter.IsActived,
                Slug = chapter.Slug,
                Status = chapter.Status,
                CreatedAt = chapter.CreatedAt,
                UpdatedAt = chapter.UpdatedAt,
            };
            return chapterItemRes;
        }

        public ChapterRes MapChapterRes(Chapter chapter)
        {
            var pathList = new List<string>();
            if(chapter.ChapterImages != null)
            {
                foreach (var chapterImage in chapter.ChapterImages)
                {
                    pathList.Add(_fileService.GetFileUrl(chapterImage.Path));
                }
            }
            var chapterRes = new ChapterRes()
            {
                Id = chapter.Id,
                StoryId = chapter.StoryId,
                Name = chapter.Name,
                Title = chapter.Title,
                MetaDescription = chapter.MetaDescription,
                MetaKeyword = chapter.MetaKeyword,
                Comment = chapter.Comment,
                View = chapter.View,
                Index = chapter.Index,
                HotFlag = chapter.HotFlag,
                IsActived = chapter.IsActived,
                Slug = chapter.Slug,
                Status = chapter.Status,
                CreatedAt = chapter.CreatedAt,
                UpdatedAt = chapter.UpdatedAt,
                Images = pathList,
            };
            return chapterRes;
        }
    }
}
