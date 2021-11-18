using Article_Project.DataLayer.Context;
using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.GenericRepository.Interface;
using Article_Project.Services.Repositories.GenericRepository.Service;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Services.Repositories.UnitOfWorkRepository.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataBaseContext _context;

        public IGenericRepository<Post> PostRepository { get; }
        public IGenericRepository<Follow> FollowRepository { get; }
        public IGenericRepository<Comment> CommentRepository { get; }
        public IGenericRepository<Like> LikeRepository { get; }
        public IGenericRepository<Bookmark> BookmarkRepository { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<Role> RoleRepository { get; }

        public UnitOfWork(DataBaseContext context)
        {
            _context = context;

            BookmarkRepository = new GenericRepository<Bookmark>(_context);
            LikeRepository = new GenericRepository<Like>(_context);
            PostRepository = new GenericRepository<Post>(_context);
            CommentRepository = new GenericRepository<Comment>(_context);
            FollowRepository = new GenericRepository<Follow>(_context);
            RoleRepository = new GenericRepository<Role>(_context);
            UserRepository = new GenericRepository<User>(_context);
        }
    }
}
