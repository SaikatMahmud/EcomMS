﻿using AutoMapper;
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
    public class OrderStatusHistoryService
    {
        private readonly IUnitOfWork DataAccess;
        public OrderStatusHistoryService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<OrderStatusHistoryDTO> Get(string? properties = null)
        {
            var data = DataAccess.OrderStatusHistory.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<OrderStatusHistory, OrderStatusHistoryDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<OrderStatusHistoryDTO>>(data);

            }
            return null;
        }

        public List<OrderStatusHistoryDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.OrderStatusHistory.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<OrderStatusHistory, OrderStatusHistoryDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<OrderStatusHistoryDTO>>(data.Item1);

            }
            return null;
        }

        public List<OrderStatusHistoryDTO> GetCustomized(Expression<Func<OrderStatusHistoryDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderStatusHistory, OrderStatusHistoryDTO>();
            });
            var mapper = new Mapper(cfg);
            var orderStatusHistoryFilter = mapper.MapExpression<Expression<Func<OrderStatusHistory, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.OrderStatusHistory.GetCustomizedListData(orderStatusHistoryFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<OrderStatusHistoryDTO>>(data.Item1);

            }
            return null;
        }

        public OrderStatusHistoryDTO Get(Expression<Func<OrderStatusHistoryDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderStatusHistory, OrderStatusHistoryDTO>();
            });

            var mapper = new Mapper(cfg);
            var orderStatusHistoryFilter = mapper.MapExpression<Expression<Func<OrderStatusHistory, bool>>>(filter);

            var data = DataAccess.OrderStatusHistory.Get(orderStatusHistoryFilter, properties);
            if (data != null)
            {
                return mapper.Map<OrderStatusHistoryDTO>(data);
            }
            return null;
        }
        public bool Create(OrderStatusHistoryDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<OrderStatusHistoryDTO, OrderStatusHistory>();
            });
            var mapper = new Mapper(cfg);
            var OrderStatusHistory = mapper.Map<OrderStatusHistory>(obj);
            return DataAccess.OrderStatusHistory.Create(OrderStatusHistory);
        }
        //public bool Update(OrderStatusHistoryDTO obj)
        //{
        //    var existingData = DataAccess.OrderStatusHistory.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.OrderStatusHistory.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.OrderStatusHistory.Get(c => c.Id == Id);
            return DataAccess.OrderStatusHistory.Delete(data);
        }
    }
}