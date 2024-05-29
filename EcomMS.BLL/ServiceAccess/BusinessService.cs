using EcomMS.BLL.SendEmail;
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
        public UserService UserService { get; set; }
        public MailService MailService { get; set; }

        public BusinessService(IUnitOfWork unitOfWork, IMailService mailService)
        {
            CartService = new CartService(unitOfWork);
            CategoryService = new CategoryService(unitOfWork);
            CustomerService = new CustomerService(unitOfWork);
            EmployeeService = new EmployeeService(unitOfWork);
            OrderProductService = new OrderProductService(unitOfWork);
            OrderService = new OrderService(unitOfWork, mailService);
            OrderStatusHistoryService = new OrderStatusHistoryService(unitOfWork);
            ProductImageService = new ProductImageService(unitOfWork);
            ProductService = new ProductService(unitOfWork);
            ReviewService = new ReviewService(unitOfWork);
            SupplierProductService = new SupplierProductService(unitOfWork);
            SupplierService = new SupplierService(unitOfWork);
            UserService = new UserService(unitOfWork);
        }
    }
}
