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
    public class CategoryService
    {
        private readonly IUnitOfWork DataAccess;
        public CategoryService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<CategoryDTO> Get(string? properties = null)
        {
            var data = DataAccess.Category.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Category, CategoryDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<CategoryDTO>>(data);

            }
            return null;
        }

        public List<CategoryDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Category.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Category, CategoryDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CategoryDTO>>(data.Item1);

            }
            return null;
        }

        public List<CategoryDTO> GetCustomized(Expression<Func<CategoryDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Category, CategoryDTO>();
            });
            var mapper = new Mapper(cfg);
            var categoryFilter = mapper.MapExpression<Expression<Func<Category, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Category.GetCustomizedListData(categoryFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CategoryDTO>>(data.Item1);

            }
            return null;
        }

        public CategoryDTO Get(Expression<Func<CategoryDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Category, CategoryDTO>();
            });

            var mapper = new Mapper(cfg);
            var categoryFilter = mapper.MapExpression<Expression<Func<Category, bool>>>(filter);

            var data = DataAccess.Category.Get(categoryFilter, properties);
            if (data != null)
            {
                return mapper.Map<CategoryDTO>(data);
            }
            return null;
        }
        public bool Create(CategoryDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<CategoryDTO, Category>();
            });
            var mapper = new Mapper(cfg);
            var Category = mapper.Map<Category>(obj);
            return DataAccess.Category.Create(Category);
        }
        public bool Update(CategoryDTO obj)
        {
            var existingData = DataAccess.Category.Get(c => c.Id == obj.Id);
            if (existingData != null)
            {
                existingData.Name = obj.Name;
                existingData.Description = obj.Description;
            }
            return DataAccess.Category.Update(existingData);
        }
        public bool Delete(int Id)
        {
            var data = DataAccess.Category.Get(c => c.Id == Id);
            return DataAccess.Category.Delete(data);
        }

        public async Task<bool> UploadFromExcel(Stream stream)
        {
            var data = DataUploadService.ParseCategoryData(stream);
            if (data != null)
            {
                var result = await DataAccess.Category.UploadBulk(data);
                if (result.success) return true;
            }
            return false;

        }
    }
}
