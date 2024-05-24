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
    public class CustomerService
    {
        private readonly IUnitOfWork DataAccess;
        public CustomerService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<CustomerDTO> Get(string? properties = null)
        {
            var data = DataAccess.Customer.Get(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Customer, CustomerDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<CustomerDTO>>(data);

            }
            return null;
        }

        public List<CustomerDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Customer.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Customer, CustomerDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CustomerDTO>>(data.Item1);

            }
            return null;
        }

        public List<CustomerDTO> GetCustomized(Expression<Func<CustomerDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Customer, CustomerDTO>();
            });
            var mapper = new Mapper(cfg);
            var customerFilter = mapper.MapExpression<Expression<Func<Customer, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Customer.GetCustomizedListData(customerFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CustomerDTO>>(data.Item1);

            }
            return null;
        }

        public CustomerDTO Get(Expression<Func<CustomerDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Customer, CustomerDTO>();
            });

            var mapper = new Mapper(cfg);
            var customerFilter = mapper.MapExpression<Expression<Func<Customer, bool>>>(filter);

            var data = DataAccess.Customer.Get(customerFilter, properties);
            if (data != null)
            {
                return mapper.Map<CustomerDTO>(data);
            }
            return null;
        }
        public bool Create(CustomerDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<CustomerDTO, Customer>();
            });
            var mapper = new Mapper(cfg);
            var Customer = mapper.Map<Customer>(obj);
            return DataAccess.Customer.Create(Customer);
        }
        //public bool Update(CustomerDTO obj)
        //{
        //    var existingData = DataAccess.Customer.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.Customer.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.Customer.Get(c => c.Id == Id);
            return DataAccess.Customer.Delete(data);
        }

    }
}
