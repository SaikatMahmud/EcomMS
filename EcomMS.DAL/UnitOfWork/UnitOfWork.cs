using EcomMS.DAL.Interfaces;
using EcomMS.DAL.Models;
using EcomMS.DAL.Repos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ICart Cart { get; private set; }

        public ICategory Category { get; private set; }

        public ICustomer Customer { get; private set; }

        public IEmployee Employee { get; private set; }

        public IOrder Order { get; private set; }

        public IOrderProduct OrderProduct { get; private set; }

        public IOrderStatusHistory OrderStatusHistory { get; private set; }

        public IProduct Product { get; private set; }

        public IProductImage ProductImage { get; private set; }

        public IReview Review { get; private set; }

        public ISupplier Supplier { get; private set; }

        public ISupplierProduct SupplierProduct { get; private set; }
        public IUser<User> User { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Cart = new CartRepo(_db);
            Category = new CategoryRepo(_db);
            Customer = new CustomerRepo(_db);
            Employee = new EmployeeRepo(_db);
            Order = new OrderRepo(_db);
            OrderProduct = new OrderProductRepo(_db);
            OrderStatusHistory = new OrderStatusHistoryRepo(_db);
            Product = new ProductRepo(_db);
            ProductImage = new ProductImageRepo(_db);
            Review = new ReviewRepo(_db);
            Supplier = new SupplierRepo(_db);
            SupplierProduct = new SupplierProductRepo(_db);
            User = new UserRepo(_db);
        }
    }
}
