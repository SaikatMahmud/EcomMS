using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Models
{
    public class Order
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
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; }


    }
}
