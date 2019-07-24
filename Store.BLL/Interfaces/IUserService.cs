using System;
using System.Threading.Tasks;
using Store.Domain.Entities;

namespace Store.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserAsync(string login, string password);

    }
}