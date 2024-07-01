using Comic.Application.Common;
using Comic.Application.IServices;
using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.CategoryModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.StoryModel;
using Comic.Domain.UnitOfWork;

namespace Comic.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CategoryReq categoryReq)
        {
            try
            {
                var newCategory = new Category()
                {
                    Name = categoryReq.Name,
                    Description = categoryReq.Description,
                    MetaKeyword = categoryReq.MetaKeyword,
                    MetaDescription = categoryReq.MetaDescription,
                    Type = (CategoryType)categoryReq.Type,
                    Slug = SeoHelper.GenerateSlug(categoryReq.Name),
                    IsActived = (bool)categoryReq.IsActived,
                };
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.CategoryRepository.AddAsync(newCategory);
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
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.CategoryRepository.Delete(category);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message,ex);
            }
        }

        public async Task<Category> GetAsync(string id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục");
            }
            return category;
        }

        public async Task<DatatableRes<Category>> GetDatatableAsync(DatatableReq datatableReq)
        {
            var categoryTotal = await _unitOfWork.CategoryRepository.GetAllAsync();
            // get categories
            ICollection<Category> categories;
            var orderColumn = datatableReq.Order.FirstOrDefault();
            if (orderColumn.Column.Equals("name"))
            {
                categories = await _unitOfWork.CategoryRepository.GetAllDatatableAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.Name, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("updatedAt"))
            {
                categories = await _unitOfWork.CategoryRepository.GetAllDatatableAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.UpdatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("createdAt"))
            {
                categories = await _unitOfWork.CategoryRepository.GetAllDatatableAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.CreatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else
            {
                categories = await _unitOfWork.CategoryRepository.GetAllDatatableAsync(
                  q => q.Name.Contains(datatableReq.Search.Value)
                  , q => q.CreatedAt, "desc", datatableReq.Start, datatableReq.Length);
            }
            var datatableRes = new DatatableRes<Category>()
            {
                Draw = datatableReq.Draw,
                RecordsTotal = categoryTotal.Count,
                RecordsFiltered = categoryTotal.Count,
                Data = categories,
            };
            return datatableRes;
        }

        public async Task<ICollection<Category>> GetListAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllSortAsync(q => q.Name, true);
            return categories;
        }

        public async Task UpdateAsync(string id, CategoryReq categoryReq)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                category.Name = categoryReq.Name;
                category.Description = categoryReq.Description;
                category.MetaKeyword = categoryReq.MetaKeyword;
                category.MetaDescription = categoryReq.MetaDescription;
                category.Type = (CategoryType)categoryReq.Type;
                category.IsActived = (bool)categoryReq.IsActived;
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        #region Client
        public async Task<ICollection<Category>> GetActivedAllAsync()
        {
            var categories = await _unitOfWork.CategoryRepository
                .GetAllAsync(q=>q.IsActived == true, q => q.Name, true);
            return categories;
        }
        #endregion

    }
}
