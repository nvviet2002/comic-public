using Amazon.S3.Model;
using Comic.Application.Common;
using Comic.Application.IServices;
using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.CategoryModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.StoryModel;
using Comic.Domain.ResponseModels.UserModel;
using Comic.Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;


namespace Comic.Application.Services
{
    public class StoryService : IStoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IChapterService _chapterService;
        private readonly IMapperService _mapperService;

        public StoryService(IUnitOfWork unitOfWork, IFileService fileService
            , IChapterService chapterService, IMapperService mapperService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _chapterService = chapterService;
            _mapperService = mapperService;
        }

        public async Task CreateAsync(StoryReq storyReq)
        {
            try
            {
                var newStory = new Story()
                {
                    Name = storyReq.Name,
                    OtherName = storyReq.OtherName,
                    Description = storyReq.Description,
                    MetaKeyword = storyReq.MetaKeyword,
                    MetaDescription = storyReq.MetaDescription,
                    Status = (StoryStatus)storyReq.Status,
                    Slug = SeoHelper.GenerateSlug(storyReq.Name),
                    View = 0,
                    Comment = 0,
                    Rate = 1,
                    RateTotal = 0,
                    Follow = 0,
                    HotFlag = (bool)storyReq.HotFlag,
                    IsActived = (bool)storyReq.IsActived,
                };
                //create avatar
                if (storyReq.Avatar != null)
                {
                    var avatarName = Guid.NewGuid().ToString();
                    var avatarPath = await _fileService.UploadImageAsync(storyReq.Avatar, avatarName, "upload/story/avatar");
                    newStory.Avatar = avatarPath;
                }
                var storyCategoryList = new List<StoryCategory>();
                foreach (var categoryId in storyReq.CategoryIds)
                {
                    var newStoryCategory = new StoryCategory()
                    {
                        CategoryId = categoryId,
                        StoryId = newStory.Id,
                    };
                    storyCategoryList.Add(newStoryCategory);
                }
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.StoryRepository.AddAsync(newStory);
                await _unitOfWork.StoryCategoryRepository.AddRangeAsync(storyCategoryList);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();

                //create story folder
                await _fileService.CreateFolderAsync(newStory.Id, "upload/story");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var story = await _unitOfWork.StoryRepository.GetByIdAsync(id);
            if (story == null)
            {
                throw new NotFoundException("Không tìm thấy truyện");
            }
            try
            {
                var rates = await _unitOfWork.RateRepository.GetAllAsync(q=>q.StoryId == story.Id);
                var histories = await _unitOfWork.HistoryRepository.GetAllAsync(q => q.StoryId == story.Id);
                var follows = await _unitOfWork.FollowRepository.GetAllAsync(q => q.StoryId == story.Id);
                var storyCategorys = await _unitOfWork.StoryCategoryRepository.GetAllAsync(q => q.StoryId == story.Id);
                var chapters = await _unitOfWork.ChapterRepository.GetAllAsync(q => q.StoryId == story.Id);
                foreach (var chapter in chapters)
                {
                    await _chapterService.DeleteAsync(chapter.Id);
                }
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.RateRepository.DeleteRange(rates);
                _unitOfWork.HistoryRepository.DeleteRange(histories);
                _unitOfWork.FollowRepository.DeleteRange(follows);
                _unitOfWork.StoryCategoryRepository.DeleteRange(storyCategorys);
                _unitOfWork.StoryRepository.Delete(story);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
                //delete story folder
                await _fileService.DeleteFolderAsync(id, "upload/story");
                //delete story avatar
                await _fileService.DeleteFileAsync(story.Avatar);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<StoryRes> GetAsync(string id)
        {
            var story = await _unitOfWork.StoryRepository.GetByQuery()
                .Include(q=>q.StoryCategories).ThenInclude(q=>q.Category)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (story == null)
            {
                throw new NotFoundException("Không tìm thấy truyện");
            }
            return _mapperService.MapStoryRes(story);
        }

        public async Task<DatatableRes<StoryItemRes>> GetDatatableAsync(DatatableReq datatableReq)
        {
            var storyTotal = (await _unitOfWork.StoryRepository.GetAllAsync()).Count;
            // get stories
            ICollection<Story> stories;
            var orderColumn = datatableReq.Order.FirstOrDefault();
            if (orderColumn.Column.Equals("name"))
            {
                stories = await _unitOfWork.StoryRepository.GetAllDatatableIncludeCategoriesAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.Name, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("view"))
            {
                stories = await _unitOfWork.StoryRepository.GetAllDatatableIncludeCategoriesAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.View, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("updatedAt"))
            {
                stories = await _unitOfWork.StoryRepository.GetAllDatatableIncludeCategoriesAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.UpdatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("createdAt"))
            {
                stories = await _unitOfWork.StoryRepository.GetAllDatatableIncludeCategoriesAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.CreatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else
            {
                stories = await _unitOfWork.StoryRepository.GetAllDatatableIncludeCategoriesAsync(
                  q => q.Name.Contains(datatableReq.Search.Value)
                  , q => q.CreatedAt, "desc", datatableReq.Start, datatableReq.Length);
            }
            //format story item response
            var storyItemResList = new List<StoryItemRes>();
            foreach(var story in stories)
            {
                storyItemResList.Add(_mapperService.MapStoryItemRes(story));
            }
            var datatableRes = new DatatableRes<StoryItemRes>()
            {
                Draw = datatableReq.Draw,
                RecordsTotal = storyTotal,
                RecordsFiltered = storyTotal,
                Data = storyItemResList,
            };
            return datatableRes;
        }

        public async Task<ICollection<Story>> GetListAsync()
        {
            var stories = await _unitOfWork.StoryRepository.GetByQuery()
                .Include(q=>q.StoryCategories).ThenInclude(q=>q.Category)
                .ToListAsync();
            return stories;
        }

        public async Task UpdateAsync(string id, StoryReq storyReq)
        {
            var story = await _unitOfWork.StoryRepository.GetByIdAsync(id);
            if (story == null)
            {
                throw new NotFoundException("Không tìm thấy truyện");
            }
            try
            {
                story.Name = storyReq.Name;
                story.OtherName = storyReq.OtherName;
                story.Description = storyReq.Description;
                story.MetaKeyword = storyReq.MetaKeyword;
                story.MetaDescription = storyReq.MetaDescription;
                story.Status = (StoryStatus)storyReq.Status;
                story.Slug = SeoHelper.GenerateSlug(storyReq.Name);
                story.HotFlag = (bool)storyReq.HotFlag;
                story.IsActived = (bool)storyReq.IsActived;
                //update avatar
                if (storyReq.Avatar != null)
                {
                    await _fileService.DeleteFileAsync(story.Avatar);
                    var avatarName = $"{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
                    var avatarPath = await _fileService.UploadImageAsync(storyReq.Avatar, avatarName, "upload/story/avatar");
                    story.Avatar = avatarPath;
                }
                //update categories
                var storyCategoryList = new List<StoryCategory>();
                foreach (var categoryId in storyReq.CategoryIds)
                {
                    var newStoryCategory = new StoryCategory()
                    {
                        CategoryId = categoryId,
                        StoryId = story.Id,
                    };
                    storyCategoryList.Add(newStoryCategory);
                }
                var oldStoryCategoryList = await _unitOfWork.StoryCategoryRepository.GetAllAsync(q=>q.StoryId == story.Id);
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.StoryRepository.Update(story);
                _unitOfWork.StoryCategoryRepository.DeleteRange(oldStoryCategoryList);
                await _unitOfWork.StoryCategoryRepository.AddRangeAsync(storyCategoryList);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task RefreshViewAsync(string id)
        {
            var story = await _unitOfWork.StoryRepository.GetByIdAsync(id);
            
            if (story == null)
            {
                throw new NotFoundException("Không tìm thấy truyện");
            }
            var chapters = await _unitOfWork.ChapterRepository
                .GetAllAsync(q => q.IsActived == true && q.StoryId == id);
            try
            {
                var random = new Random();
                story.View = chapters.Sum(q=>q.View);
                story.Comment = chapters.Sum(q => q.Comment);
                story.Rate = (float)Math.Round(Math.Clamp(random.NextDouble() * 5, 3, 5),1);
                story.Follow = (int)Math.Ceiling((decimal)story.View / random.Next(25, 50));
                story.RateTotal = (int)Math.Ceiling((decimal)story.View / random.Next(25, 50));
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.StoryRepository.Update(story);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        #region client
        public async Task<PaginateRes<StoryRes>> GetListPagingAsync(PaginateReq paginateReq
            , Expression<Func<Story, bool>> filter, Expression<Func<Story, dynamic>> orderBy
            , bool ascendingSort)
        {
            var stories = new List<Story>();
            var rawStories = _unitOfWork.StoryRepository.GetByQuery().Where(filter);
            var totalRows = await rawStories.CountAsync();
            rawStories = rawStories
                .Include(q => q.StoryCategories)
                    .ThenInclude(q => q.Category)
                .Include(q => q.Chapters.Where(c => c.IsActived).OrderByDescending(c => c.Index).Take(3))
                    .ThenInclude(c => c.ChapterImages.OrderBy(ci => ci.Index));

            if (ascendingSort)
            {
                stories = await rawStories.OrderBy(orderBy).Skip((paginateReq.PageNumber - 1) * paginateReq.PageSize)
                    .Take(paginateReq.PageSize).ToListAsync();
            }
            else
            {
                stories = await rawStories.OrderByDescending(orderBy).Skip((paginateReq.PageNumber - 1) * paginateReq.PageSize)
                    .Take(paginateReq.PageSize).ToListAsync();
            }

            if (stories == null)
            {
                throw new NotFoundException("Không tìm thấy danh sách truyện");
            }
            var newStoryResList = new List<StoryRes>();
            foreach (var story in stories)
            {
                newStoryResList.Add(_mapperService.MapStoryRes(story));
            }
            var newPaginateRes = new PaginateRes<StoryRes>()
            {
                TotalCount = totalRows,
                PageNumber = paginateReq.PageNumber,
                PageSize = paginateReq.PageSize,
                PageCount = stories.Count,
                TotalPages = (int)Math.Ceiling((decimal)totalRows / paginateReq.PageSize),
                Items = newStoryResList
            };

            return newPaginateRes;
        }

        public async Task<StoryRes> GetDetailAsync(string id)
        {
            var story = await _unitOfWork.StoryRepository.GetByQuery()
                .Include(q => q.StoryCategories)
                    .ThenInclude(q => q.Category)
                .Include(q => q.Chapters.Where(c => c.IsActived).OrderByDescending(c => c.Index))
                    .ThenInclude(c => c.ChapterImages.OrderBy(ci => ci.Index))
                .FirstOrDefaultAsync(q => q.Id == id);
            if (story == null)
            {
                throw new NotFoundException("Không tìm thấy truyện");
            }
            var storyRes = _mapperService.MapStoryRes(story);
            return storyRes;
        }

        public async Task<PaginateRes<StoryRes>> SearchPagingAsync(PaginateReq paginateReq
            , Expression<Func<Story, bool>> filter
            , Expression<Func<Story, dynamic>> orderBy, bool ascendingSort)
        {
            var stories = new List<Story>();
            var rawStories = _unitOfWork.StoryRepository.GetByQuery().Where(filter);
            var totalRows = await rawStories.CountAsync();
            rawStories = rawStories
                .Include(q => q.StoryCategories)
                    .ThenInclude(q => q.Category)
                .Include(q => q.Chapters.Where(c => c.IsActived).OrderByDescending(c => c.Index).Take(3))
                    .ThenInclude(c => c.ChapterImages.OrderBy(ci => ci.Index));

            if (ascendingSort)
            {
                stories = await rawStories.OrderBy(orderBy).Skip((paginateReq.PageNumber - 1) * paginateReq.PageSize)
                    .Take(paginateReq.PageSize).ToListAsync();
            }
            else
            {
                stories = await rawStories.OrderByDescending(orderBy).Skip((paginateReq.PageNumber - 1) * paginateReq.PageSize)
                    .Take(paginateReq.PageSize).ToListAsync();
            }

            if (stories == null)
            {
                throw new NotFoundException("Không tìm thấy danh sách truyện");
            }
            var newStoryResList = new List<StoryRes>();
            foreach (var story in stories)
            {
                newStoryResList.Add(_mapperService.MapStoryRes(story));
            }
            var newPaginateRes = new PaginateRes<StoryRes>()
            {
                TotalCount = totalRows,
                PageNumber = paginateReq.PageNumber,
                PageSize = paginateReq.PageSize,
                PageCount = stories.Count,
                TotalPages = (int)Math.Ceiling((decimal)totalRows / paginateReq.PageSize),
                Items = newStoryResList
            };

            return newPaginateRes;
        }

        public async Task<ICollection<StoryRes>> GetAllAsync(
            Expression<Func<Story, bool>> filter
            ,Expression<Func<Story, dynamic>> orderBy
            , bool ascendingSort)
        {
            var stories = new List<Story>();
            var rawStories = _unitOfWork.StoryRepository.GetByQuery().Where(filter);
            rawStories = rawStories
                .Include(q => q.StoryCategories)
                    .ThenInclude(q => q.Category)
                .Include(q => q.Chapters.Where(c => c.IsActived).OrderByDescending(c => c.Index));




            if (ascendingSort)
            {
                stories = await rawStories.OrderBy(orderBy).ToListAsync();
            }
            else
            {
                stories = await rawStories.OrderByDescending(orderBy).ToListAsync();
            }

            if (stories == null)
            {
                throw new NotFoundException("Không tìm thấy danh sách truyện");
            }
            var newStoryResList = new List<StoryRes>();
            foreach (var story in stories)
            {
                newStoryResList.Add(_mapperService.MapStoryRes(story));
            }

            return newStoryResList;
        }
        #endregion

    }
}
