using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.SendEmail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        bool OrderConfirmationEmail(int customerId, int orderId, int amount);
    }
}
