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
    public class ProductService
    {
        private readonly IUnitOfWork DataAccess;
        public ProductService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<ProductDTO> Get(string? properties = null)
        {
            var data = DataAccess.Product.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Product, ProductDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<ProductDTO>>(data);

            }
            return null;
        }

        public List<ProductDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Product.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Product, ProductDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ProductDTO>>(data.Item1);

            }
            return null;
        }

        public List<ProductDTO> GetCustomized(Expression<Func<ProductDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Product, ProductDTO>();
            });
            var mapper = new Mapper(cfg);
            var productFilter = mapper.MapExpression<Expression<Func<Product, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Product.GetCustomizedListData(productFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ProductDTO>>(data.Item1);

            }
            return null;
        }

        public ProductDTO Get(Expression<Func<ProductDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Product, ProductDTO>();
            });

            var mapper = new Mapper(cfg);
            var productFilter = mapper.MapExpression<Expression<Func<Product, bool>>>(filter);

            var data = DataAccess.Product.Get(productFilter, properties);
            if (data != null)
            {
                return mapper.Map<ProductDTO>(data);
            }
            return null;
        }
        public bool Create(ProductDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ProductDTO, Product>();
            });
            var mapper = new Mapper(cfg);
            var Product = mapper.Map<Product>(obj);
            return DataAccess.Product.Create(Product);
        }
        //public bool Update(ProductDTO obj)
        //{
        //    var existingData = DataAccess.Product.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.Product.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.Product.Get(c => c.Id == Id);
            return DataAccess.Product.Delete(data);
        }

    }
}
