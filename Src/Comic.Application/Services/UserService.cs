using Comic.Application.IServices;
using Comic.Domain.Entities;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.UserModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.User;
using Comic.Domain.ResponseModels.UserModel;
using Comic.Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Comic.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IFileService _fileService;
        private readonly RoleManager<Role> _roleManager;

        public UserService(IUnitOfWork unitOfWork, IFileService fileService
            , UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _fileService = fileService;
            _roleManager = roleManager;
        }

        public async Task CreateAsync(CreateUserReq userReq)
        {
            try
            {
                var newUser = new User()
                {
                    Email = userReq.Email,
                    UserName = userReq.Email,
                    Name = userReq.Name,
                    Birthday = userReq.BirthDay,
                    Point = (decimal)userReq.Point,
                    PhoneNumber = userReq.PhoneNumber,
                    IsActived = (bool)userReq.IsActived,
                    Avatar = string.Empty,
                };
                //create avatar
                if (userReq.Avatar != null)
                {
                    var avatarName = $"{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
                    var avatarPath = await _fileService.UploadImageAsync(userReq.Avatar, avatarName,"upload/user/avatar");
                    newUser.Avatar = avatarPath;
                }
                var userResult = await _userManager.CreateAsync(newUser,userReq.Password);
                //assign roles for new user
                if(userResult.Succeeded)
                {
                    foreach(var roleId in userReq.RoleIds)
                    {
                        var role = await _roleManager.FindByIdAsync(roleId);
                        var roleResult = await _userManager.AddToRoleAsync(newUser, role.Name);
                        if (!roleResult.Succeeded)
                        {
                            throw new AppException("Gán vai trò cho người dùng thất bại");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }

        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng");
            }
            var result = await _userManager.DeleteAsync(user);
            if(!result.Succeeded)
            {
                throw new AppException("Xóa người dùng thất bại");
            }
        }
        public async Task<UserRes> GetAsync(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng");
            }
            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = new List<IdentityRole>();
            foreach(var name in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(name);
                roles.Add(role);
            }
            var userRes = new UserRes()
            {
                Id = user.Id,
                Name = user.Name,
                Birthday = user.Birthday,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Point = user.Point,
                IsActived = user.IsActived,
                UpdatedAt = user.UpdatedAt,
                CreatedAt = user.CreatedAt,
                Avatar = _fileService.GetFileUrl(user.Avatar),
                Roles = roles,
            };
            return userRes;

        }

        public async Task<DatatableRes<UserItemRes>> GetDatatableAsync(DatatableReq datatableReq)
        {
            var userTotal = await _unitOfWork.UserRepository.GetAllAsync();
            // get users
            ICollection<User> users;
            var orderColumn = datatableReq.Order.FirstOrDefault();
            if (orderColumn.Column.Equals("name"))
            {
                users = await _unitOfWork.UserRepository.GetAllDatatableAsync(
                   q => q.Email.Contains(datatableReq.Search.Value)
                   , q => q.Name, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("email"))
            {
                users = await _unitOfWork.UserRepository.GetAllDatatableAsync(
                   q => q.Email.Contains(datatableReq.Search.Value)
                   , q => q.Email, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("point"))
            {
                users = await _unitOfWork.UserRepository.GetAllDatatableAsync(
                   q => q.Email.Contains(datatableReq.Search.Value)
                   , q => q.Point, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("updatedAt"))
            {
                users = await _unitOfWork.UserRepository.GetAllDatatableAsync(
                   q => q.Email.Contains(datatableReq.Search.Value)
                   , q => q.UpdatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else if (orderColumn.Column.Equals("createdAt"))
            {
                users = await _unitOfWork.UserRepository.GetAllDatatableAsync(
                   q => q.Email.Contains(datatableReq.Search.Value)
                   , q => q.CreatedAt, orderColumn.Dir, datatableReq.Start, datatableReq.Length);
            }
            else
            {
                users = await _unitOfWork.UserRepository.GetAllDatatableAsync(
                  q => q.Email.Contains(datatableReq.Search.Value)
                  , q => q.CreatedAt, "desc", datatableReq.Start, datatableReq.Length);
            }
            //get role names
            var newUserItemResList = new List<UserItemRes>();
            foreach(var user in users)
            {
                var newUserItemRes = new UserItemRes()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Birthday = user.Birthday,
                    IsActived = user.IsActived,
                    PhoneNumber = user.PhoneNumber,
                    Point = user.Point,
                    UpdatedAt = user.UpdatedAt,
                    CreatedAt = user.CreatedAt,
                    RoleNames = await _userManager.GetRolesAsync(user),
                };
                newUserItemResList.Add(newUserItemRes);
            }
            var datatableRes = new DatatableRes<UserItemRes>()
            {
                Draw = datatableReq.Draw,
                RecordsTotal = userTotal.Count,
                RecordsFiltered = userTotal.Count,
                Data = newUserItemResList,
            };

            return datatableRes;
        }

        public Task<ICollection<User>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(string id, UpdateUserReq userReq)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng");
            }
            //change password
            if (userReq.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userReq.Password);
                if (!result.Succeeded)
                {
                    throw new AppException("Đổi mật khẩu mới không thành công");
                }
            }
            user.Email = userReq.Email;
            user.UserName = userReq.Email;
            user.Name = userReq.Name;
            user.Birthday = userReq.BirthDay;
            user.Point = (decimal)userReq.Point;
            user.PhoneNumber = userReq.PhoneNumber;
            user.IsActived = (bool)userReq.IsActived;

            //change avatar
            if (userReq.Avatar != null)
            {
                await _fileService.DeleteFileAsync(user.Avatar);
                var avatarName = Guid.NewGuid().ToString();
                var avatarPath = await _fileService.UploadImageAsync(userReq.Avatar, avatarName, "upload/user/avatar");
                user.Avatar = avatarPath;
            }

            //assign roles for new user
            var assignedRoles = await _userManager.GetRolesAsync(user);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, assignedRoles);
            foreach (var roleId in userReq.RoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
                if (!roleResult.Succeeded)
                {
                    throw new AppException("Gán vai trò cho người dùng thất bại");
                }
            }

        }

    }
}
