using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.DTOs
{
    public class OrderWithStatusHistoriesDTO : OrderDTO
    {
        public List<OrderStatusHistoryDTO> OrderStatusHistories { get; set; }
    }
}
