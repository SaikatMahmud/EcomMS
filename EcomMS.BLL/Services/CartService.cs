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
    public class CartService
    {
        private readonly IUnitOfWork DataAccess;
        public CartService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<CartDTO> Get(string? properties = null)
        {
            var data = DataAccess.Cart.GetAll(properties);
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

        public List<CartProductImagesDTO> GetCartWithProduct(Expression<Func<CartDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Cart, CartDTO>();
                c.CreateMap<Cart, CartProductImagesDTO>();
                c.CreateMap<Product, ProductImageMapDTO>();
                c.CreateMap<ProductImage, ProductImageDTO>();
            });

            var mapper = new Mapper(cfg);
            var cartFilter = mapper.MapExpression<Expression<Func<Cart, bool>>>(filter);

            var data = DataAccess.Cart.GetAll(cartFilter, properties);
            if (data != null)
            {
                return mapper.Map<List<CartProductImagesDTO>>(data);
            }
            return null;
        }
        public List<CartDTO> GetAll(Expression<Func<CartDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Cart, CartDTO>();
                c.CreateMap<Product, ProductDTO>();
            });

            var mapper = new Mapper(cfg);
            var cartFilter = mapper.MapExpression<Expression<Func<Cart, bool>>>(filter);

            var data = DataAccess.Cart.GetAll(cartFilter, properties);
            if (data != null)
            {
                return mapper.Map<List<CartDTO>>(data);
            }
            return null;
        }
        public bool AddItemToCart(CartDTO obj, out string msg)
        {
            msg = "";
            if(obj.Quantity > 5)
            {
                msg = "Maximum quantity is 5";
                return false;
            }
            var currentCart = DataAccess.Cart.Get(c => c.ProductId == obj.ProductId && c.CustomerId == obj.CustomerId);
            var productInventory = DataAccess.Product.Get(p => p.Id == obj.ProductId);
            if (currentCart != null)
            {
                currentCart.Quantity += obj.Quantity;
                if (currentCart.Quantity > 5)
                {
                    msg = $"Max quantity is 5. You are trying to add {currentCart.Quantity}";
                    return false;
                }  
                else if (currentCart.Quantity > productInventory.Quantity)
                {
                    msg = "Product has lower inventory than cart quantity";
                    return false;
                }   
                DataAccess.Cart.Update(currentCart);
                msg = $"{currentCart.Quantity} of the product in cart now!";
            }
            else
            {
                if (obj.Quantity > productInventory.Quantity)
                {
                    msg = "Product has lower inventory than cart quantity";
                    return false;
                }
                Create(obj);
                msg = $"{obj.Quantity} of the product added in cart!";
            }
            return true;
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
        public bool Deduct(int cartId)
        {
            var existingData = DataAccess.Cart.Get(c => c.Id == cartId);
            if (existingData.Quantity != 1)
            {
                existingData.Quantity -= 1;
                var result = DataAccess.Cart.Update(existingData);
            }
            return true;
        }
        public bool Increase(int cartId)
        {
            var existingData = DataAccess.Cart.Get(c => c.Id == cartId);
            if (existingData.Quantity < 5)
            {
                existingData.Quantity += 1;
                var result = DataAccess.Cart.Update(existingData);
            }
            return true;
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
