using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class ProductRepo : Repository<Product>, IProduct
    {
        private readonly ApplicationDbContext _db;
        public ProductRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<(bool success, string? error)> UploadBulk(List<Product> products)
        {
            await _db.Products.AddRangeAsync(products);
            await _db.SaveChangesAsync();
            return (true, null);
        }
    }
}
