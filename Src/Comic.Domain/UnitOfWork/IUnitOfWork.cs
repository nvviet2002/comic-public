using Comic.Domain.Repositories;

namespace Comic.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task BeginTransactionAsync();
        Task SaveChangeAsync();
        Task RollBackAsync();

        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get;}
        IRefreshTokenRepository RefreshTokenRepository { get;}
        IPermissionRepository PermissionRepository { get;}
        IRolePermissionRepository RolePermissionRepository { get;}
        ICategoryRepository CategoryRepository { get;}
        IStoryRepository StoryRepository { get;}
        IStoryCategoryRepository StoryCategoryRepository { get;}
        IChapterRepository ChapterRepository { get;}
        IChapterImageRepository ChapterImageRepository { get;}
        IFollowRepository FollowRepository { get;}
        IRateRepository RateRepository { get;}
        IHistoryRepository HistoryRepository { get;}
        ICommentLv1Repository CommentLv1Repository { get;}
        ICommentLv2Repository CommentLv2Repository { get;}
    }
}
