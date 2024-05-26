using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsDelivered { get; set; }
        //public string Status { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CustomerDTO Customer { get; set; }


    }
}
