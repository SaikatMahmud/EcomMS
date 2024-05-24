using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.DTOs
{
    public class OrderStatusHistoryDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderDTO Order { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
