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
    public class SupplierService
    {
        private readonly IUnitOfWork DataAccess;
        public SupplierService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<SupplierDTO> Get(string? properties = null)
        {
            var data = DataAccess.Supplier.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Supplier, SupplierDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<SupplierDTO>>(data);

            }
            return null;
        }

        public List<SupplierDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Supplier.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Supplier, SupplierDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<SupplierDTO>>(data.Item1);

            }
            return null;
        }

        public List<SupplierDTO> GetCustomized(Expression<Func<SupplierDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Supplier, SupplierDTO>();
            });
            var mapper = new Mapper(cfg);
            var supplierFilter = mapper.MapExpression<Expression<Func<Supplier, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Supplier.GetCustomizedListData(supplierFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<SupplierDTO>>(data.Item1);

            }
            return null;
        }

        public SupplierDTO Get(Expression<Func<SupplierDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Supplier, SupplierDTO>();
            });

            var mapper = new Mapper(cfg);
            var supplierFilter = mapper.MapExpression<Expression<Func<Supplier, bool>>>(filter);

            var data = DataAccess.Supplier.Get(supplierFilter, properties);
            if (data != null)
            {
                return mapper.Map<SupplierDTO>(data);
            }
            return null;
        }
        public bool Create(SupplierDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<SupplierDTO, Supplier>();
            });
            var mapper = new Mapper(cfg);
            var Supplier = mapper.Map<Supplier>(obj);
            return DataAccess.Supplier.Create(Supplier);
        }
        //public bool Update(SupplierDTO obj)
        //{
        //    var existingData = DataAccess.Supplier.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.Supplier.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.Supplier.Get(c => c.Id == Id);
            return DataAccess.Supplier.Delete(data);
        }
    }
}
