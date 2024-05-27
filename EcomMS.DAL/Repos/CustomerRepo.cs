using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class CustomerRepo : Repository<Customer>, ICustomer
    {
        private readonly ApplicationDbContext _db;
        public CustomerRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Customer Create(Customer obj)
        {
            _db.Customers.Add(obj);
            if (_db.SaveChanges() > 0) return obj;
            return null;
        }
    }
}
