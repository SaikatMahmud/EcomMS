using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        //public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int DelManRating { get; set; }
        public OrderDTO Order { get; set; }

    }
}
