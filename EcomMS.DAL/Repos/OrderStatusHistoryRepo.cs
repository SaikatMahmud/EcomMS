using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class OrderStatusHistoryRepo : Repository<OrderStatusHistory>, IOrderStatusHistory
    {
        private readonly ApplicationDbContext _db;
        public OrderStatusHistoryRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
