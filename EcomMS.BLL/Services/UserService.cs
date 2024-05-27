using AutoMapper;
using EcomMS.BLL.DTOs;
using EcomMS.DAL.Models;
using EcomMS.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.Services
{
    public class UserService
    {
        private readonly IUnitOfWork DataAccess;
        public UserService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public UserCustomerDTO LoginCustomer(string username, string password)
        {
            var data = DataAccess.User.Get(username, password, "Customer");
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<User, UserCustomerDTO>();
                    c.CreateMap<Customer, CustomerDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<UserCustomerDTO>(data);
            }
            return null;
        }
        public bool RegisterCustomer(CustomerDTO obj, string username, string password, out string emailUnique, out string usernameUnique)
        {
            emailUnique = string.Empty;
            usernameUnique = string.Empty;
            var uniqueUsername = DataAccess.User.IsUsernameUnique(username);
            if (!uniqueUsername)
            {
                usernameUnique = "Username not available!";
                return false;
            }
            var uniqueEmail = DataAccess.Customer.Get(c => c.Email == obj.Email);
            if(uniqueEmail != null)
            {
                emailUnique = "Email already taken!";
                return false;
            }
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<CustomerDTO, Customer>();
            });
            var mapper = new Mapper(cfg);
            var newCustomer =  mapper.Map<Customer>(obj);

            var createdCustomer = DataAccess.Customer.Create(newCustomer);
            if (createdCustomer != null)
            {
                var user = new User()
                {
                    Username = username,
                    Password = password,
                    Type = "Customer",
                    CustomerId = createdCustomer.Id
                };
                var result = DataAccess.User.Create(user);
                return result != null ? true : false;
            }
            return false;
        }
    }
}
