using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using Comic.Domain.UnitOfWork;
using Comic.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;


namespace Comic.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ComicDbContext _comicDbContext;
        private readonly ILogger _logger;

        private IDbContextTransaction _transaction;

        //define repositories
        public IUserRepository UserRepository { get; private set ; }
        public IRoleRepository RoleRepository { get; private set; }
        public IPermissionRepository PermissionRepository { get; private set; }
        public IRolePermissionRepository RolePermissionRepository { get; private set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IStoryRepository StoryRepository { get; private set; }
        public IStoryCategoryRepository StoryCategoryRepository { get; private set; }
        public IChapterRepository ChapterRepository { get; private set; }
        public IChapterImageRepository ChapterImageRepository { get; private set; }
        public IFollowRepository FollowRepository { get; private set; }
        public IRateRepository RateRepository { get; private set; }
        public IHistoryRepository HistoryRepository { get; private set; }
        public ICommentLv1Repository CommentLv1Repository { get; private set; }
        public ICommentLv2Repository CommentLv2Repository { get; private set; }


        public UnitOfWork(ComicDbContext comicDbContext, ILogger<UnitOfWork> logger)
        {
            _comicDbContext = comicDbContext;
            _logger = logger;
            UserRepository = new UserRepository(_comicDbContext);
            RoleRepository = new RoleRepository(_comicDbContext);
            PermissionRepository = new PermissionRepository(_comicDbContext);
            RolePermissionRepository = new RolePermissionRepository(_comicDbContext);
            RefreshTokenRepository = new RefreshTokenRepository(_comicDbContext);
            CategoryRepository = new CategoryRepository(_comicDbContext);
            StoryRepository = new StoryRepository(_comicDbContext);
            StoryCategoryRepository = new StoryCategoryRepository(_comicDbContext);
            ChapterRepository = new ChapterRepository(_comicDbContext);
            ChapterImageRepository = new ChapterImageRepository(_comicDbContext);
            FollowRepository = new FollowRepository(_comicDbContext);
            RateRepository = new RateRepository(_comicDbContext);
            HistoryRepository = new HistoryRepository(_comicDbContext);
            CommentLv1Repository = new CommentLv1Repository(_comicDbContext);
            CommentLv2Repository = new CommentLv2Repository(_comicDbContext);
    }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _comicDbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }
        public async Task SaveChangeAsync()
        {
            await _comicDbContext.SaveChangesAsync();
        }
        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _comicDbContext.Dispose();
        }

    }
}
