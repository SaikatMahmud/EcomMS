using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Models
{
    public class SupplierProduct
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
