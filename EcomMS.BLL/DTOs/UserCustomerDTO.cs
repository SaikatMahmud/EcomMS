using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.DTOs
{
    public class UserCustomerDTO : UserDTO
    {
        public CustomerDTO Customer { get; set; }
    }
}
