using Microsoft.EntityFrameworkCore;
using Store.BLL.Interfaces;
using Store.DAL;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.BLL.Services
{
    public sealed class UserService : EntitiesService, IUserService
    {
        private DataBaseContext Context { get; set; }
        public UserService(DataBaseContext context) : base(context)
        {
            Context = context;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var user = await GetByIdAsync(id,
                new List<Expression<Func<User, object>>>
                {
                    x => x.Roles
                });

            return user;
        }

        public async Task<User> GetUserAsync(string login, string password)
        {
            var user = await Context.Users.Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            return user;
        }
    }
}