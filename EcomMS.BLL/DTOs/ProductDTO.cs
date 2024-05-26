using EcomMS.DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int ReorderQuantity { get; set; }
        public bool IsLive { get; set; }
        public string? Description { get; set; }
        public string? Specification { get; set; }
        public int? CategoryId { get; set; }
        [ValidateNever]
        public CategoryDTO Category { get; set; }


    }
}
