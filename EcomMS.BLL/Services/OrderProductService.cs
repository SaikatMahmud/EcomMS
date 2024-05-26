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
    public class OrderProductService
    {
        private readonly IUnitOfWork DataAccess;
        public OrderProductService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<OrderProductDTO> Get(string? properties = null)
        {
            var data = DataAccess.OrderProduct.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<OrderProduct, OrderProductDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<OrderProductDTO>>(data);

            }
            return null;
        }

        public List<OrderProductDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.OrderProduct.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<OrderProduct, OrderProductDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<OrderProductDTO>>(data.Item1);

            }
            return null;
        }

        public List<OrderProductDTO> GetCustomized(Expression<Func<OrderProductDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderProduct, OrderProductDTO>();
            });
            var mapper = new Mapper(cfg);
            var orderProductFilter = mapper.MapExpression<Expression<Func<OrderProduct, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.OrderProduct.GetCustomizedListData(orderProductFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<OrderProductDTO>>(data.Item1);

            }
            return null;
        }

        public OrderProductDTO Get(Expression<Func<OrderProductDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderProduct, OrderProductDTO>();
            });

            var mapper = new Mapper(cfg);
            var orderProductFilter = mapper.MapExpression<Expression<Func<OrderProduct, bool>>>(filter);

            var data = DataAccess.OrderProduct.Get(orderProductFilter, properties);
            if (data != null)
            {
                return mapper.Map<OrderProductDTO>(data);
            }
            return null;
        }
        public List<OrderProductDTO> GetAll(Expression<Func<OrderProductDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderProduct, OrderProductDTO>();
                c.CreateMap<Product, ProductDTO>();
            });

            var mapper = new Mapper(cfg);
            var orderProductFilter = mapper.MapExpression<Expression<Func<OrderProduct, bool>>>(filter);

            var data = DataAccess.OrderProduct.GetAll(orderProductFilter, properties);
            if (data != null)
            {
                return mapper.Map<List<OrderProductDTO>>(data);
            }
            return null;
        }
        public bool Create(OrderProductDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderProductDTO, OrderProduct>();
            });
            var mapper = new Mapper(cfg);
            var OrderProduct = mapper.Map<OrderProduct>(obj);
            return DataAccess.OrderProduct.Create(OrderProduct);
        }
        //public bool Update(OrderProductDTO obj)
        //{
        //    var existingData = DataAccess.OrderProduct.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.OrderProduct.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.OrderProduct.Get(c => c.Id == Id);
            return DataAccess.OrderProduct.Delete(data);
        }
    }
}
