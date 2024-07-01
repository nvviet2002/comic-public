using Comic.Application.IServices;
using Comic.Domain.Entities;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(string name)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var permission = new Permission()
                {
                    Name = name,
                    NormalizedName = name.ToUpper(),
                };
                await _unitOfWork.PermissionRepository.AddAsync(permission);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(id);
            if(permission == null)
            {
                throw new NotFoundException("Không tìm thấy quyền");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.PermissionRepository.Delete(permission);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }catch(Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<Permission> GetAsync(string id)
        {

            var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(id);
            if (permission == null)
            {
                throw new NotFoundException("Không tìm thấy quyền");
            }
            return permission;
           
        }

        

        public async Task<ICollection<Permission>> GetListAsync()
        {
            var permissions = await _unitOfWork.PermissionRepository
                .GetAllSortAsync(q=>q.Name,true);
            if(permissions == null)
            {
                throw new NotFoundException("Không tìm thấy danh sách quyền");
            }
            return permissions;
        }
        public async Task<PaginateRes<Permission>> GetPaginatedListAsync(PaginateReq paginateReq)
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAllPaginateAsync(
                q=>q.Name.Contains(paginateReq.SearchTerm),q=>q.Name,true,paginateReq);
            if (permissions == null)
            {
                throw new NotFoundException("Không tìm thấy danh sách quyền");
            }
            return permissions;
        }
        public async Task<DatatableRes<Permission>> GetDatatableAsync(DatatableReq datatableReq)
        {
            
            var permissionTotal = await _unitOfWork.PermissionRepository.GetAllAsync();

            ICollection<Permission> permissions;
            var orderColumn = datatableReq.Order.FirstOrDefault();
            if (orderColumn.Column == "name")
            {
                permissions = await _unitOfWork.PermissionRepository.GetAllDatatableAsync(
                    q => q.Name.Contains(datatableReq.Search.Value)
                    , q => q.Name, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else
            {
                permissions = await _unitOfWork.PermissionRepository.GetAllDatatableAsync(
                   q => q.Name.Contains(datatableReq.Search.Value)
                   , q => q.Name, "asc", datatableReq.Start, datatableReq.Length);
            }
            var datatableRes = new DatatableRes<Permission>()
            {
                Draw = datatableReq.Draw,
                RecordsTotal = permissionTotal.Count,
                RecordsFiltered = permissionTotal.Count,
                Data = permissions,
            };

            return datatableRes;
        }

        public async Task UpdateAsync(string id, string name)
        {
            var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(id);
            if(permission == null)
            {
                throw new NotFoundException("Không tìm thấy quyền");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                permission.Name = name;
                permission.NormalizedName = name.ToUpper();
                _unitOfWork.PermissionRepository.Update(permission);
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
