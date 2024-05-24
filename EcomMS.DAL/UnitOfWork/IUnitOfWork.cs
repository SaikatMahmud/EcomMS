using EcomMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICart Cart { get; }
        ICategory Category { get; }
        ICustomer Customer { get; }
        IEmployee Employee { get; }
        IOrder Order { get; }
        IOrderProduct OrderProduct { get; }
        IOrderStatusHistory OrderStatusHistory { get; }
        IProduct Product { get; }
        IProductImage ProductImage { get; }
        IReview Review { get; }
        ISupplier Supplier { get; }
        ISupplierProduct SupplierProduct { get; }
    }
}
