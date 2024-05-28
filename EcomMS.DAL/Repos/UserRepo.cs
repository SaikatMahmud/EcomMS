using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class UserRepo : IUser<User>
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<string> pw;
        public UserRepo(ApplicationDbContext db)
        {
            _db = db;
            pw = new PasswordHasher<string>();
        }

        public User Create(User user)
        {
            var hashedPassword = pw.HashPassword(user.Username, user.Password);
            user.Password = hashedPassword;
            _db.Users.Add(user);
            if (_db.SaveChanges() > 0) return user;
            return null;
        }

        public User Get(string username, string password, string? includeProperties = null)
        {
            IQueryable<User> query = _db.Users;
            query = query.Where(u=> u.Username == username);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            var user = query.FirstOrDefault();
            var verificationResult = pw.VerifyHashedPassword(username, user.Password, password);
            if (verificationResult == PasswordVerificationResult.Success)
            {
                user.Password = "*";
                return user;
            }
            return null;
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
