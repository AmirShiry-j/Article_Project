using Article_Project.Services.Repositories.GenericRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Article_Project.DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace Article_Project.Services.Repositories.GenericRepository.Service
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DataBaseContext _context;
        private DbSet<T> _table;

        public GenericRepository(DataBaseContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> Expression = null,
                                                        params Expression<Func<T, object>>[] Includes)
        {
            IQueryable<T> table = _table;
            if (Expression != null)
            {
                table = table.Where(Expression);
            }

            if (Includes != null)
            {
                foreach (Expression<Func<T, object>> item in Includes)
                {
                    table = table.Include(item);
                }
            }

            return await table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object Id)
        {
            return await _table.FindAsync(Id);
        }

        public async Task<bool> AddAsync(T Item)
        {
            try
            {
                await _table.AddAsync(Item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddRangeAsync(List<T> Items)
        {
            try
            {
                await _table.AddRangeAsync(Items);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAsync(T Item)
        {
            try
            {
                _table.Remove(Item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveRangeAsync(List<T> Items)
        {
            try
            {
                _table.RemoveRange(Items);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveByIdAsync(object Id)
        {
            try
            {
                T item = await GetByIdAsync(Id);
                return await RemoveAsync(item);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T Item)
        {
            try
            {
                _context.Entry(Item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public T this[int Index]
        {
            get
            {
                T[] data = _table.ToArray();
                if (0 <= Index && Index < data.Length)
                {
                    return data[Index];
                }
                else
                {
                    throw new ArgumentException("Index was not found !");
                }
            }
        }
    }
}
