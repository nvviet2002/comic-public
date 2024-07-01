using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Comic.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Comic.Infrastructure.Data
{
    public class ComicDbContext: IdentityDbContext<User,Role,string>
    {
        public ComicDbContext(DbContextOptions<ComicDbContext> options) : base(options)
        {
            //...
        }

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (!string.IsNullOrEmpty(tableName) && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Replace("AspNet", string.Empty));
                }
            }
            modelBuilder.Entity<Chapter>().Property(q => q.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<Story>().Property(q => q.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<Category>().Property(q => q.Type).HasConversion<string>().HasMaxLength(50);

        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryCategory> StoryCategories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ChapterImage> ChapterImages { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<CommentLv1> CommentLv1s { get; set; }
        public DbSet<CommentLv2> CommentLv2s { get; set; }


    }
}
