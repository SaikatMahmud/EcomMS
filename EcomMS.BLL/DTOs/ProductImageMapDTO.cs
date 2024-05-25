using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.DTOs
{
    public class ProductImageMapDTO : ProductDTO
    {
        public List<ProductImageDTO> Images { get; set; }
    }
}
