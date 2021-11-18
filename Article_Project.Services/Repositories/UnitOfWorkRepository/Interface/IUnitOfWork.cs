using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.GenericRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Services.Repositories.UnitOfWorkRepository.Interface
{
    public interface IUnitOfWork
    {
        IGenericRepository<Post> PostRepository { get; }
        IGenericRepository<Follow> FollowRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<Like> LikeRepository { get; }
        IGenericRepository<Bookmark> BookmarkRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }

    }
}
