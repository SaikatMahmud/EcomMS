using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using EcomMS.BLL.DTOs;
using EcomMS.BLL.SendEmail;
using EcomMS.BLL.ViewModels;
using EcomMS.DAL.Models;
using EcomMS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork DataAccess;
        private readonly IMailService _mailService;
        public OrderService(IUnitOfWork _dataAccess, IMailService mailService)
        {
            DataAccess = _dataAccess;
            _mailService = mailService;
        }
        public List<OrderWithStatusHistoriesDTO> Get(string? properties = null)
        {
            var data = DataAccess.Order.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Order, OrderWithStatusHistoriesDTO>();
                    c.CreateMap<OrderStatusHistory, OrderStatusHistoryDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<OrderWithStatusHistoriesDTO>>(data);

            }
            return null;
        }

        public List<OrderWithStatusHistoriesDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Order.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Order, OrderWithStatusHistoriesDTO>();
                    c.CreateMap<OrderStatusHistory, OrderStatusHistoryDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                var result = mapper.Map<List<OrderWithStatusHistoriesDTO>>(data.Item1);
                result.ForEach(o => o.OrderStatusHistories = o.OrderStatusHistories.OrderByDescending(h => h.CreatedAt).ToList());
                return result;
            }
            return null;
        }

        public List<OrderWithStatusHistoriesDTO> GetCustomized(Expression<Func<OrderDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Order, OrderDTO>();
                c.CreateMap<Order, OrderWithStatusHistoriesDTO>();
                c.CreateMap<OrderStatusHistory, OrderStatusHistoryDTO>();
            });
            var mapper = new Mapper(cfg);
            var orderFilter = mapper.MapExpression<Expression<Func<Order, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Order.GetCustomizedListData(orderFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                var result = mapper.Map<List<OrderWithStatusHistoriesDTO>>(data.Item1);
                result.ForEach(o => o.OrderStatusHistories = o.OrderStatusHistories.OrderByDescending(h => h.CreatedAt).ToList());
                return result;
            }
            return null;
        }

        public OrderDTO Get(Expression<Func<OrderDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Order, OrderDTO>();
            });

            var mapper = new Mapper(cfg);
            var orderFilter = mapper.MapExpression<Expression<Func<Order, bool>>>(filter);

            var data = DataAccess.Order.Get(orderFilter, properties);
            if (data != null)
            {
                return mapper.Map<OrderDTO>(data);
            }
            return null;
        }
        //public bool Create(OrderDTO obj)
        //{
        //    var cfg = new MapperConfiguration(c =>
        //    {
        //        c.CreateMap<OrderDTO, Order>();
        //    });
        //    var mapper = new Mapper(cfg);
        //    var order = mapper.Map<Order>(obj);
        //    return DataAccess.Order.Create(order);
        //}
        //public bool Update(OrderDTO obj)
        //{
        //    var existingData = DataAccess.Order.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.Order.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.Order.Get(c => c.Id == Id);
            return DataAccess.Order.Delete(data);
        }

        public bool PlaceOrder(CartSummaryVM obj, out string msg)
        {
            msg = string.Empty;
            //first checking if any product has lower inventory than the cart quantity
            var cartItems = DataAccess.Cart.GetAll(c => c.CustomerId == obj.CustomerId, "Product");
            List<Product> productsToBeOrdered = new List<Product>();
            foreach(var cart in cartItems)
            {
               var product = DataAccess.Product.Get(p => p.Id == cart.ProductId);
                if(product.Quantity < cart.Quantity)
                {
                    msg = product.Name + " has lower inventory than cart quantity";
                    return false;
                }
                productsToBeOrdered.Add(product);
            }
            //if ok then proceed to place order
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<CartSummaryVM, Order>();
            });
            var mapper = new Mapper(cfg);
            var order = mapper.Map<Order>(obj);
            order.IsDelivered = false;
            order.IsReviewed = false;
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            var createdOrder = DataAccess.Order.Create(order);
            if(createdOrder != null)
            {
                foreach(var cart in cartItems)
                {
                    var product = (from pToOrder in productsToBeOrdered
                               where pToOrder.Id == cart.ProductId
                               select pToOrder).FirstOrDefault();
                    var op = new OrderProduct()
                    {
                        OrderId = createdOrder.Id,
                        ProductId = cart.ProductId,
                        Price = product.Price,
                        Quantity = cart.Quantity
                    };
                    DataAccess.OrderProduct.Create(op);
                    product.Quantity -= cart.Quantity;
                    DataAccess.Product.Update(product);
                }
                var orderStatus = new OrderStatusHistory()
                {
                    OrderId = createdOrder.Id,
                    Status = "Placed",
                    CreatedAt = DateTime.Now
                };
                DataAccess.OrderStatusHistory.Create(orderStatus);
                var cartRemoved = DataAccess.Cart.DeleleByCustomerId(obj.CustomerId);
                _mailService.OrderConfirmationEmail(obj.CustomerId, createdOrder.Id, createdOrder.Amount);
                if (cartRemoved)
                {
                    msg = "Order placed successfully!";
                    return true;
                }
            }
            msg = "Internal server error";
            return false;
        }       
    }
}
