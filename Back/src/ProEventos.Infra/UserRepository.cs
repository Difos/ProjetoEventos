using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Infra.Context;
using ProEventos.Infra.Interfaces;

namespace ProEventos.Infra
{
    public class UserRepository : IUserRepository, IRepository
    {
        public readonly InfraContext _context;
        public UserRepository(InfraContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            throw new System.NotImplementedException();
        }

        public void Delete<T>(T entity) where T : class
        {
            throw new System.NotImplementedException();
        }

        public void DeleteRange<T>(T[] entity) where T : class
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(int Id)
        {
           return await _context.Users.FindAsync(Id);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
             return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public void Insert<T>(T entity) where T : class
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Update<T>(T entity) where T : class
        {
            throw new System.NotImplementedException();
        }

    }
}