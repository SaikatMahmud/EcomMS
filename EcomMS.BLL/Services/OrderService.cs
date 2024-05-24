using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using EcomMS.BLL.DTOs;
using EcomMS.DAL.Models;
using EcomMS.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork DataAccess;
        public OrderService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<OrderDTO> Get(string? properties = null)
        {
            var data = DataAccess.Order.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Order, OrderDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<OrderDTO>>(data);

            }
            return null;
        }

        public List<OrderDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Order.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Order, OrderDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<OrderDTO>>(data.Item1);

            }
            return null;
        }

        public List<OrderDTO> GetCustomized(Expression<Func<OrderDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Order, OrderDTO>();
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
                return mapper.Map<List<OrderDTO>>(data.Item1);

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
        public bool Create(OrderDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderDTO, Order>();
            });
            var mapper = new Mapper(cfg);
            var Order = mapper.Map<Order>(obj);
            return DataAccess.Order.Create(Order);
        }
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
    }
}
