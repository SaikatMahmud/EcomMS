using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class CategoryRepo : Repository<Category>, ICategory
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<(bool success, string? error)> UploadBulk(List<Category> categories)
        {
            await _db.Categories.AddRangeAsync(categories);
            await _db.SaveChangesAsync();
            return (true, null);
        }
    }
}
