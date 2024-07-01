using Comic.Application.Common;
using Comic.Application.IServices;
using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.ChapterModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.StoryModel;
using Comic.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Comic.Application.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IMapperService _mapperService;

        public ChapterService(IUnitOfWork unitOfWork, IFileService fileService
            , IMapperService mapperService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _mapperService = mapperService;
        }

        public async Task CreateAsync(string storyId , ChapterReq chapterReq)
        {
            try
            {
                var random = new Random();
                var newChapter = new Chapter()
                {
                    Name = chapterReq.Name,
                    Title = chapterReq.Title,
                    MetaKeyword = chapterReq.MetaKeyword,
                    MetaDescription = chapterReq.MetaDescription,
                    Status = (ChapterStatus)chapterReq.Status,
                    Slug = SeoHelper.GenerateSlug(chapterReq.Name),
                    View = (int)chapterReq.View,
                    Index = (int)chapterReq.Index,
                    Comment = (int)Math.Ceiling((decimal)chapterReq.View / random.Next(6, 19)),
                    HotFlag = (bool)chapterReq.HotFlag,
                    IsActived = (bool)chapterReq.IsActived,
                    StoryId = storyId,
                };
                //create images
                var newChapterImages = new List<ChapterImage>();
                if (chapterReq.Images != null)
                {
                    
                    for (int i = 0; i < chapterReq.Images.Count ; i++)
                    {
                        var imageName = Guid.NewGuid().ToString();
                        var imagePath = await _fileService
                            .UploadImageAsync(chapterReq.Images[i], imageName, $"upload/story/{storyId}");
                        //create new chapter image
                        var newChapterImage = new ChapterImage()
                        {
                            ChapterId = newChapter.Id,
                            Path = imagePath,
                            Index = i,
                        };
                        newChapterImages.Add(newChapterImage);
                    }
                }

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.ChapterRepository.AddAsync(newChapter);
                await _unitOfWork.ChapterImageRepository.AddRangeAsync(newChapterImages);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByQuery()
                .Include(q => q.ChapterImages)
                .Include(q => q.CommentLv1s).ThenInclude(q => q.CommentLv2s)
                .FirstOrDefaultAsync(q=>q.Id == id);
            if (chapter == null)
            {
                throw new NotFoundException("Không tìm thấy chapter");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.ChapterImageRepository.DeleteRange(chapter.ChapterImages);
                foreach(var commentLv1 in chapter.CommentLv1s)
                {
                    _unitOfWork.CommentLv2Repository.DeleteRange(commentLv1.CommentLv2s);
                }
                _unitOfWork.CommentLv1Repository.DeleteRange(chapter.CommentLv1s);
                _unitOfWork.ChapterRepository.Delete(chapter);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
                //delete image of chapter
                foreach(var chapterImage in chapter.ChapterImages)
                {
                    await _fileService.DeleteFileAsync(chapterImage.Path);
                }
                
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<ChapterRes> GetAsync(string id)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByQuery()
                .Include(q => q.ChapterImages.OrderBy(c=>c.Index)).FirstOrDefaultAsync(q => q.Id == id);
            if (chapter == null)
            {
                throw new NotFoundException("Không tìm thấy chapter");
            }
            return _mapperService.MapChapterRes(chapter);
        }

        public async Task<DatatableRes<ChapterItemRes>> GetDatatableAsync(string storyId, DatatableReq datatableReq)
        {
            var chapterTotal = (await _unitOfWork.ChapterRepository.GetAllAsync(q=>q.StoryId == storyId)).Count;
            // get stories
            ICollection<Chapter> chapters;
            var orderColumn = datatableReq.Order.FirstOrDefault();
            if (orderColumn.Column.Equals("index"))
            {
                chapters = await _unitOfWork.ChapterRepository.GetAllDatatableAsync(
                    q => q.StoryId == storyId && q.Name.Contains(datatableReq.Search.Value)
                   , q => q.Index, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("name"))
            {
                chapters = await _unitOfWork.ChapterRepository.GetAllDatatableAsync(
                    q=>q.StoryId == storyId && q.Name.Contains(datatableReq.Search.Value)
                   , q => q.Name, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("view"))
            {
                chapters = await _unitOfWork.ChapterRepository.GetAllDatatableAsync(
                    q => q.StoryId == storyId && q.Name.Contains(datatableReq.Search.Value)
                   , q => q.View, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("comment"))
            {
                chapters = await _unitOfWork.ChapterRepository.GetAllDatatableAsync(
                   q => q.StoryId == storyId && q.Name.Contains(datatableReq.Search.Value)
                   , q => q.Comment, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("updatedAt"))
            {
                chapters = await _unitOfWork.ChapterRepository.GetAllDatatableAsync(
                   q => q.StoryId == storyId && q.Name.Contains(datatableReq.Search.Value)
                   , q => q.UpdatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("createdAt"))
            {
                chapters = await _unitOfWork.ChapterRepository.GetAllDatatableAsync(
                   q => q.StoryId == storyId && q.Name.Contains(datatableReq.Search.Value)
                   , q => q.CreatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else
            {
                chapters = await _unitOfWork.ChapterRepository.GetAllDatatableAsync(
                  q => q.StoryId == storyId && q.Name.Contains(datatableReq.Search.Value)
                  , q => q.CreatedAt, "desc", datatableReq.Start, datatableReq.Length);
            }
            //format story item response
            var chapterItemResList = new List<ChapterItemRes>();
            foreach (var chapter in chapters)
            {
                
                chapterItemResList.Add(_mapperService.MapChapterItemRes(chapter));
            }
            var datatableRes = new DatatableRes<ChapterItemRes>()
            {
                Draw = datatableReq.Draw,
                RecordsTotal = chapterTotal,
                RecordsFiltered = chapterTotal,
                Data = chapterItemResList,
            };
            return datatableRes;
        }

        public async Task<ICollection<Story>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Chapter> UpdateAsync(string id, ChapterReq chapterReq)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByQuery()
                .Include(q => q.ChapterImages).FirstOrDefaultAsync(q => q.Id == id);
            if (chapter == null)
            {
                throw new NotFoundException("Không tìm thấy chapter");
            }
            try
            {
                chapter.Name = chapterReq.Name;
                chapter.Title = chapterReq.Title;
                chapter.MetaKeyword = chapterReq.MetaKeyword;
                chapter.MetaDescription = chapterReq.MetaDescription;
                chapter.Status = (ChapterStatus)chapterReq.Status;
                chapter.Slug = SeoHelper.GenerateSlug(chapterReq.Name);
                chapter.View = (int)chapterReq.View;
                chapter.Index = (int)chapterReq.Index;
                chapter.HotFlag = (bool)chapterReq.HotFlag;
                chapter.IsActived = (bool)chapterReq.IsActived;
                chapter.UpdatedAt = DateTime.UtcNow;
                //delete image of chapter
                foreach (var chapterImage in chapter.ChapterImages)
                {
                    await _fileService.DeleteFileAsync(chapterImage.Path);
                }
                //create images
                var newChapterImages = new List<ChapterImage>();
                if (chapterReq.Images != null)
                {

                    for (int i = 0; i < chapterReq.Images.Count; i++)
                    {
                        var imageName = Guid.NewGuid().ToString();
                        var imagePath = await _fileService
                            .UploadImageAsync(chapterReq.Images[i], imageName, $"upload/story/{chapter.StoryId}");
                        //create new chapter image
                        var newChapterImage = new ChapterImage()
                        {
                            ChapterId = chapter.Id,
                            Path = imagePath,
                            Index = i,
                        };
                        newChapterImages.Add(newChapterImage);
                    }
                }
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.ChapterRepository.Update(chapter);
                _unitOfWork.ChapterImageRepository.DeleteRange(chapter.ChapterImages);
                await _unitOfWork.ChapterImageRepository.AddRangeAsync(newChapterImages);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();

                return chapter;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task IncreaseViewAsync(string id, int view = 1)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
            if (chapter == null)
            {
                throw new NotFoundException("Không tìm thấy chapter");
            }
            var story = await _unitOfWork.StoryRepository.GetByIdAsync(chapter.StoryId);
            if (story == null)
            {
                throw new NotFoundException("Không tìm thấy truyện");
            }
            try
            {
                chapter.View += view;
                story.View += view;
                //save
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.ChapterRepository.Update(chapter);
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
    }
}
