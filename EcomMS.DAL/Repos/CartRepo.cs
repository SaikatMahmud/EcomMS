using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class CartRepo : Repository<Cart>, ICart
    {
        private readonly ApplicationDbContext _db;
        public CartRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public bool DeleleByCustomerId(int customerId)
        {
            var entitiesToDelete = _db.Carts.Where(c=>c.CustomerId == customerId);
            //if (entitiesToDelete.Any())
            //{
                dbSet.RemoveRange(entitiesToDelete);
                return _db.SaveChanges() > 0;
            //}
            //return false;
        }
    }
}
