using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using DocumentFormat.OpenXml.Vml;
using EcomMS.BLL.DTOs;
using EcomMS.DAL.Models;
using EcomMS.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EcomMS.BLL.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork DataAccess;
        public ProductService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<ProductImageMapDTO> Get(string? properties = null)
        {
            var data = DataAccess.Product.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Product, ProductImageMapDTO>();
                    c.CreateMap<ProductImage, ProductImageDTO>();
                    //c.CreateMap<Category, CategoryDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<ProductImageMapDTO>>(data);
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
                    c.CreateMap<Category, CategoryDTO>();
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
                c.CreateMap<Category, CategoryDTO>();
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

        public ProductImageMapDTO Get(Expression<Func<ProductDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Product, ProductDTO>();
                c.CreateMap<Product, ProductImageMapDTO>();
                c.CreateMap<ProductImage, ProductImageDTO>();
                c.CreateMap<Category, CategoryDTO>();
            });

            var mapper = new Mapper(cfg);
            var productFilter = mapper.MapExpression<Expression<Func<Product, bool>>>(filter);

            var data = DataAccess.Product.Get(productFilter, properties);
            if (data != null)
            {
                return mapper.Map<ProductImageMapDTO>(data);
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
        public bool Update(ProductDTO obj)
        {
            var existingData = DataAccess.Product.Get(c => c.Id == obj.Id);
            if (existingData != null)
            {
                existingData.Name = obj.Name;
                existingData.Price = obj.Price;
                existingData.Quantity = obj.Quantity;
                existingData.Description = obj.Description;
                existingData.Specification = obj.Specification;
                existingData.CategoryId = obj.CategoryId;
            }
            return DataAccess.Product.Update(existingData);
        }
        public bool Delete(int Id)
        {
            var data = DataAccess.Product.Get(c => c.Id == Id);
            return DataAccess.Product.Delete(data);
        }

        public async Task<bool> UploadFromExcel(Stream stream)
        {
            var data = DataUploadService.ParseProductData(stream);
            if (data != null)
            {
                var result = await DataAccess.Product.UploadBulk(data);
                if (result.success) return true;
            }
            return false;

        }
        public bool UploadImage(int Id, string ImageUrl)
        {
            var obj = new ProductImage()
            {
                ProductId = Id,
                ImageUrl = ImageUrl,
            };
            return DataAccess.ProductImage.Create(obj);
        }
        public string DeleteImage(int ImageId)
        {
            var data = DataAccess.ProductImage.Get(c => c.Id == ImageId);
            DataAccess.ProductImage.Delete(data);
            return data.ImageUrl;
        }


    }
}
