using EcomMS.BLL.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.ViewModels
{
    public class CartSummary
    {
        [ValidateNever]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [ValidateNever]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public int Amount { get; set; }
        [ValidateNever]
        public IEnumerable<CartDTO> Carts {  get; set; }
    }
}
