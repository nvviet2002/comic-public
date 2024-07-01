using Comic.Application.IServices;
using Comic.Domain.Entities;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.RoleModel;
using Comic.Domain.ResponseModels.RoleModel;
using Comic.Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Comic.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        public async Task CreateAsync(string name,ICollection<string> permissionIds)
        {
            //create role
            var newRole = new Role(name);
            var result = await _roleManager.CreateAsync(newRole);
            if(!result.Succeeded)
            {
                throw new AppException("Tạo vai trò mới thất bại");
            }
            //create role permission
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                foreach(var perId in permissionIds)
                {
                    var newRolePer = new RolePermission()
                    {
                        RoleId = newRole.Id,
                        PermissionId =perId,
                    };
                    await _unitOfWork.RolePermissionRepository.AddAsync(newRolePer);
                }
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
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException("Không tìm thấy vai trò");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var rolePermissions = await _unitOfWork.RolePermissionRepository
                    .GetAllAsync(q=>q.RoleId == id);
                if(rolePermissions != null)
                {
                    _unitOfWork.RolePermissionRepository.DeleteRange(rolePermissions);
                }
                await _roleManager.DeleteAsync(role);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<GetRoleRes> GetAsync(string id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException("Không tìm thấy vai trò");
            }
            var rolePers = await _unitOfWork.RolePermissionRepository.GetAllAsync(q => q.RoleId == id);
            var newPermissions = new List<Permission>();
            foreach (var rolePer in rolePers)
            {
                newPermissions.Add(rolePer.Permission);
            }
            var newGetRoleRes = new GetRoleRes()
            {
                Role = role,
                Permissions = newPermissions
            };

            return newGetRoleRes;
        }

        public async Task<ICollection<RoleItemRes>> GetListAsync()
        {
            try
            {
                var rolesHasCountDic = await _unitOfWork.RoleRepository.CountAllHasUserAsync();
                var roles = await _unitOfWork.RoleRepository
                    .GetAllSortAsync(q=>q.Name, true);
                var roleList = new List<RoleItemRes>();
                foreach (var role in roles)
                {
                    roleList.Add(new RoleItemRes()
                    {
                        Role = role,
                        UserCount = rolesHasCountDic.ContainsKey(role.Id)?rolesHasCountDic[role.Id]:0,
                    });
                }
                return roleList;
            }
            catch(Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task UpdateAsync(string id,RoleReq roleReq)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException("Không tìm thấy vai trò");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                //delete role-permission
                var rolePermissions = await _unitOfWork.RolePermissionRepository
                   .GetAllAsync(q => q.RoleId == role.Id);
                if (rolePermissions != null)
                {
                    _unitOfWork.RolePermissionRepository.DeleteRange(rolePermissions);
                }
                //create role-permission
                foreach (var perId in roleReq.PermissionIds)
                {
                    var newRolePer = new RolePermission()
                    {
                        RoleId = role.Id,
                        PermissionId = perId,
                    };
                    await _unitOfWork.RolePermissionRepository.AddAsync(newRolePer);
                }
                //update role
                role.Name = roleReq.Name;
                await _roleManager.UpdateAsync(role);
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
