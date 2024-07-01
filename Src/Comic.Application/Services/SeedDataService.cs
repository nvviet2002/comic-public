using Comic.Application.IServices;
using Comic.Domain.UnitOfWork;
using Microsoft.Extensions.Logging;
using Comic.Domain;
using Comic.Domain.Entities;
using Comic.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.HttpLogging;
using Comic.Domain.Contrants;

namespace Comic.Application.Services
{

    public class SeedDataService : ISeedDataService
    {
        private readonly ILogger<SeedDataService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SeedDataService(ILogger<SeedDataService> logger, IUnitOfWork unitOfWork
            ,UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitSeedDataAsync()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                //create permission
                var permissionEntities = new List<Permission>();
                foreach(var property in typeof(PermissionNames).GetProperties())
                {
                    var newPer = new Permission()
                    {
                        Name = property.GetValue(property).ToString(),
                        NormalizedName = property.GetValue(property).ToString().ToUpper(),

                    };
                    permissionEntities.Add(newPer);
                }

                await _unitOfWork.PermissionRepository.AddRangeAsync(permissionEntities);


                //create role
                var admin = new Role("Admin");
                var staff = new Role("Staff");
                var client = new Role("Client");
                await _roleManager.CreateAsync(admin);
                await _roleManager.CreateAsync(staff);
                await _roleManager.CreateAsync(client);

                //add permission for admin role
                foreach (var permission in await _unitOfWork.PermissionRepository.GetAllAsync())
                {
                    var rolePer = new RolePermission()
                    {
                        RoleId = admin.Id,
                        PermissionId = permission.Id,
                    };
                    await _unitOfWork.RolePermissionRepository.AddAsync(rolePer);
                }

                //add user
                var adminUser = new User()
                {
                    Email = "Admin@gmail.com",
                    UserName = "Admin@gmail.com",
                    Name = "Admin",
                };
                var sdsd = await _userManager.CreateAsync(adminUser, "123456");
                await _userManager.AddToRoleAsync(adminUser, "Admin");


                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }
        }
    }
}
