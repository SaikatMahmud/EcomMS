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
    public class SupplierProductService
    {
        private readonly IUnitOfWork DataAccess;
        public SupplierProductService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<SupplierProductDTO> Get(string? properties = null)
        {
            var data = DataAccess.SupplierProduct.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<SupplierProduct, SupplierProductDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<SupplierProductDTO>>(data);

            }
            return null;
        }

        public List<SupplierProductDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.SupplierProduct.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<SupplierProduct, SupplierProductDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<SupplierProductDTO>>(data.Item1);

            }
            return null;
        }

        public List<SupplierProductDTO> GetCustomized(Expression<Func<SupplierProductDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<SupplierProduct, SupplierProductDTO>();
            });
            var mapper = new Mapper(cfg);
            var supplierProductFilter = mapper.MapExpression<Expression<Func<SupplierProduct, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.SupplierProduct.GetCustomizedListData(supplierProductFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<SupplierProductDTO>>(data.Item1);

            }
            return null;
        }

        public SupplierProductDTO Get(Expression<Func<SupplierProductDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<SupplierProduct, SupplierProductDTO>();
            });

            var mapper = new Mapper(cfg);
            var supplierProductFilter = mapper.MapExpression<Expression<Func<SupplierProduct, bool>>>(filter);

            var data = DataAccess.SupplierProduct.Get(supplierProductFilter, properties);
            if (data != null)
            {
                return mapper.Map<SupplierProductDTO>(data);
            }
            return null;
        }
        public bool Create(SupplierProductDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<SupplierProductDTO, SupplierProduct>();
            });
            var mapper = new Mapper(cfg);
            var SupplierProduct = mapper.Map<SupplierProduct>(obj);
            return DataAccess.SupplierProduct.Create(SupplierProduct);
        }
        //public bool Update(SupplierProductDTO obj)
        //{
        //    var existingData = DataAccess.SupplierProduct.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.SupplierProduct.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.SupplierProduct.Get(c => c.Id == Id);
            return DataAccess.SupplierProduct.Delete(data);
        }
    }
}
