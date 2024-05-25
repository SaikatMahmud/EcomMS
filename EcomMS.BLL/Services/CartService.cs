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
    public class CartService
    {
        private readonly IUnitOfWork DataAccess;
        public CartService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<CartDTO> Get(string? properties = null)
        {
            var data = DataAccess.Cart.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Cart, CartDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<CartDTO>>(data);

            }
            return null;
        }

        public List<CartDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Cart.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Cart, CartDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CartDTO>>(data.Item1);

            }
            return null;
        }

        public List<CartDTO> GetCustomized(Expression<Func<CartDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Cart, CartDTO>();
            });
            var mapper = new Mapper(cfg);
            var cartFilter = mapper.MapExpression<Expression<Func<Cart, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Cart.GetCustomizedListData(cartFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CartDTO>>(data.Item1);

            }
            return null;
        }

        public CartDTO Get(Expression<Func<CartDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Cart, CartDTO>();
            });

            var mapper = new Mapper(cfg);
            var cartFilter = mapper.MapExpression<Expression<Func<Cart, bool>>>(filter);

            var data = DataAccess.Cart.Get(cartFilter, properties);
            if (data != null)
            {
                return mapper.Map<CartDTO>(data);
            }
            return null;
        }
        public bool Create(CartDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<CartDTO, Cart>();
            });
            var mapper = new Mapper(cfg);
            var Cart = mapper.Map<Cart>(obj);
            return DataAccess.Cart.Create(Cart);
        }
        //public bool Update(CartDTO obj)
        //{
        //    var existingData = DataAccess.Cart.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.Cart.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.Cart.Get(c => c.Id == Id);
            return DataAccess.Cart.Delete(data);
        }
    }
}