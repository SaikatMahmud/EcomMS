using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class UserRepo : IUser<User>
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<object> _passwordHasher;
        public UserRepo(ApplicationDbContext db, PasswordHasher<object> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public User Create(User user)
        {
            var hashedPassword = _passwordHasher.HashPassword(null, user.Password);
            user.Password = hashedPassword;
            _db.Users.Add(user);
            if (_db.SaveChanges() > 0) return user;
            return null;
        }

        public User Get(string username, string password, string? includeProperties = null)
        {
            password = _passwordHasher.HashPassword(null, password);
            IQueryable<User> query = _db.Users;
            query = query.Where(u=> u.Username == username && u.Password == password);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public bool IsUsernameUnique(string username)
        {
            int count = _db.Users.Where(u => u.Username == username).Count();
            return count == 0;
        }

        public User Update(User obj)
        {
            throw new NotImplementedException();
        }
    }
}
