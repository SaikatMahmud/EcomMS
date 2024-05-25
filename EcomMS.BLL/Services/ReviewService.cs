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
    public class ReviewService
    {
        private readonly IUnitOfWork DataAccess;
        public ReviewService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<ReviewDTO> Get(string? properties = null)
        {
            var data = DataAccess.Review.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Review, ReviewDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<ReviewDTO>>(data);

            }
            return null;
        }

        public List<ReviewDTO> GetCustomized(int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Review.GetCustomizedListData(skip, take, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Review, ReviewDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ReviewDTO>>(data.Item1);

            }
            return null;
        }

        public List<ReviewDTO> GetCustomized(Expression<Func<ReviewDTO, bool>> filter, int skip, int take, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Review, ReviewDTO>();
            });
            var mapper = new Mapper(cfg);
            var reviewFilter = mapper.MapExpression<Expression<Func<Review, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Review.GetCustomizedListData(reviewFilter, skip, take, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ReviewDTO>>(data.Item1);

            }
            return null;
        }

        public ReviewDTO Get(Expression<Func<ReviewDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Review, ReviewDTO>();
            });

            var mapper = new Mapper(cfg);
            var reviewFilter = mapper.MapExpression<Expression<Func<Review, bool>>>(filter);

            var data = DataAccess.Review.Get(reviewFilter, properties);
            if (data != null)
            {
                return mapper.Map<ReviewDTO>(data);
            }
            return null;
        }
        public bool Create(ReviewDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ReviewDTO, Review>();
            });
            var mapper = new Mapper(cfg);
            var Review = mapper.Map<Review>(obj);
            return DataAccess.Review.Create(Review);
        }
        //public bool Update(ReviewDTO obj)
        //{
        //    var existingData = DataAccess.Review.Get(c => c.Id == obj.Id);
        //    if (existingData != null)
        //    {
        //        existingData.Name = obj.Name;
        //    }
        //    return DataAccess.Review.Update(existingData);
        //}
        public bool Delete(int Id)
        {
            var data = DataAccess.Review.Get(c => c.Id == Id);
            return DataAccess.Review.Delete(data);
        }

    }
}
