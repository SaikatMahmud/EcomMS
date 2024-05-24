using EcomMS.BLL.Services;
using EcomMS.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.ServiceAccess
{
    public class BusinessService : IBusinessService
    {
        public CartService CartService { get; set; }
        public CategoryService CategoryService { get; set; }
        public CustomerService CustomerService { get; set; }
        public EmployeeService EmployeeService { get; set; }
        public OrderProductService OrderProductService { get; set; }
        public OrderService OrderService { get; set; }
        public OrderStatusHistoryService OrderStatusHistoryService { get; set; }
        public ProductImageService ProductImageService { get; set; }
        public ProductService ProductService { get; set; }
        public ReviewService ReviewService { get; set; }
        public SupplierProductService SupplierProductService { get; set; }
        public SupplierService SupplierService { get; set; }
        public BusinessService(IUnitOfWork unitOfWork)
        {
            
        }
    }
}
