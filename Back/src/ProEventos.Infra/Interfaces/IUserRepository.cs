using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain.Identity;

namespace ProEventos.Infra.Interfaces
{
    public interface IUserRepository : IRepository
    {
         Task<IEnumerable<User>> GetUsersAsync();

         Task<User> GetUserByIdAsync(int Id);
         Task<User> GetUserByUserNameAsync(string username);
    }
}