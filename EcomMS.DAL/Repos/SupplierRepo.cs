using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class SupplierRepo : Repository<Supplier>, ISupplier
    {
        private readonly ApplicationDbContext _db;
        public SupplierRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
