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
    public class ProductImageService
    {
        private readonly IUnitOfWork DataAccess;
        public ProductImageService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<ProductImageDTO> Get(string? properties = null)
        {
            var data = DataAccess.ProductImage.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<ProductImage, ProductImageDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<ProductImageDTO>>(data);

            }
            return null;
        }

        public List<ProductImageDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.ProductImage.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<ProductImage, ProductImageDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ProductImageDTO>>(data.Item1);

            }
            return null;
        }

        public List<ProductImageDTO> GetCustomized(Expression<Func<ProductImageDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ProductImage, ProductImageDTO>();
            });
            var mapper = new Mapper(cfg);
            var productImageFilter = mapper.MapExpression<Expression<Func<ProductImage, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.ProductImage.GetCustomizedListData(productImageFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ProductImageDTO>>(data.Item1);

            }
            return null;
        }

        public ProductImageDTO Get(Expression<Func<ProductImageDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ProductImage, ProductImageDTO>();
            });

            var mapper = new Mapper(cfg);
            var productImageFilter = mapper.MapExpression<Expression<Func<ProductImage, bool>>>(filter);

            var data = DataAccess.ProductImage.Get(productImageFilter, properties);
            if (data != null)
            {
                return mapper.Map<ProductImageDTO>(data);
            }
            return null;
        }
        public bool Create(ProductImageDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ProductImageDTO, ProductImage>();
            });
            var mapper = new Mapper(cfg);
            var ProductImage = mapper.Map<ProductImage>(obj);
            return DataAccess.ProductImage.Create(ProductImage);
        }
        //public bool Update(ProductImageDTO obj)
        //{
        //    var existingData = DataAccess.ProductImage.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.ProductImage.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.ProductImage.Get(c => c.Id == Id);
            return DataAccess.ProductImage.Delete(data);
        }
    }
}
