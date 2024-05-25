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
    public class EmployeeService
    {
        private readonly IUnitOfWork DataAccess;
        public EmployeeService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<EmployeeDTO> Get(string? properties = null)
        {
            var data = DataAccess.Employee.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Employee, EmployeeDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<EmployeeDTO>>(data);

            }
            return null;
        }

        public List<EmployeeDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Employee.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Employee, EmployeeDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<EmployeeDTO>>(data.Item1);

            }
            return null;
        }

        public List<EmployeeDTO> GetCustomized(Expression<Func<EmployeeDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Employee, EmployeeDTO>();
            });
            var mapper = new Mapper(cfg);
            var employeeFilter = mapper.MapExpression<Expression<Func<Employee, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Employee.GetCustomizedListData(employeeFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<EmployeeDTO>>(data.Item1);

            }
            return null;
        }

        public EmployeeDTO Get(Expression<Func<EmployeeDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Employee, EmployeeDTO>();
            });

            var mapper = new Mapper(cfg);
            var employeeFilter = mapper.MapExpression<Expression<Func<Employee, bool>>>(filter);

            var data = DataAccess.Employee.Get(employeeFilter, properties);
            if (data != null)
            {
                return mapper.Map<EmployeeDTO>(data);
            }
            return null;
        }
        public bool Create(EmployeeDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<EmployeeDTO, Employee>();
            });
            var mapper = new Mapper(cfg);
            var Employee = mapper.Map<Employee>(obj);
            return DataAccess.Employee.Create(Employee);
        }
        //public bool Update(EmployeeDTO obj)
        //{
        //    var existingData = DataAccess.Employee.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.Employee.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.Employee.Get(c => c.Id == Id);
            return DataAccess.Employee.Delete(data);
        }
    }
}
