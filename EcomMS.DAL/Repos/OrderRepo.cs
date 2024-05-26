using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Repos
{
    internal class OrderRepo : Repository<Order>, IOrder
    {
        private readonly ApplicationDbContext _db;
        public OrderRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Order Create(Order order)
        {
            _db.Orders.Add(order);
            if (_db.SaveChanges() > 0) return order;
            return null;
        }
    }
}
