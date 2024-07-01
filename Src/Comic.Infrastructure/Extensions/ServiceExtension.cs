using Comic.Domain.Entities;
using Comic.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Comic.Domain.Common;
using Comic.Domain.UnitOfWork;
using Comic.Infrastructure.Data.UnitOfWork;
using Comic.Domain.Exceptions;
using Comic.Application.IServices;
using Comic.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Comic.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddComicDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ComicDbContext>(options =>
            {
                options.UseSqlServer(AppSetting.DefaultConnection,
                    opts => { opts.MigrationsAssembly(typeof(ComicDbContext).Assembly.FullName); });
            });

        }

        public static void AddComicIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User,Role>().AddEntityFrameworkStores<ComicDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Config password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true; // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = false; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

            });
        }
        public static void LoadAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            AppSetting.DefaultConnection = configuration.GetValue<string?>("AppSettings:DefaultConnection");
            AppSetting.AppName = configuration.GetValue<string?>("AppSettings:AppName");
            AppSetting.JWTValidIssuer = configuration.GetValue<string?>("JWT:ValidIssuer");
            AppSetting.JWTValidAudience = configuration.GetValue<string?>("JWT:ValidAudience");
            AppSetting.JWTSecretKey = configuration.GetValue<string?>("JWT:SecretKey");
            AppSetting.JWTATExpireTime = configuration.GetValue<double>("JWT:ATExpireTime");
            AppSetting.JWTRTExpireTime = configuration.GetValue<double>("JWT:RTExpireTime");
            AppSetting.FileRootUrl = configuration.GetValue<string?>("File:RootUrl");
            AppSetting.AwsAccessKey = configuration.GetValue<string?>("AwsS3:AccessKey");
            AppSetting.AwsSecretKey = configuration.GetValue<string?>("AwsS3:SecretKey");
            AppSetting.AwsBucketName = configuration.GetValue<string?>("AwsS3:BucketName");
            AppSetting.AwsServiceUrl = configuration.GetValue<string?>("AwsS3:ServiceUrl");

        }

        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ISeedDataService, SeedDataService>();
            services.AddScoped<IFileService, AwsS3Service>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<IMapperService, MapperService>();


        }

        public static void AddAppAuthetication(this IServiceCollection services)
        {
            //Config JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.Name = "AccessToken";
                options.Cookie.Name = "RefreshToken";
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = AppSetting.JWTValidAudience,
                    ValidIssuer = AppSetting.JWTValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.JWTSecretKey)),
                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = (context) =>
                    {
                        var authorizationToken = context.Request.Headers["Authorization"];
                        if (authorizationToken.IsNullOrEmpty())
                        {
                            context.Token = context.Request.Cookies["AccessToken"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

    }
}
