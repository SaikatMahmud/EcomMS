using EcomMS.BLL.SendEmail;
using EcomMS.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.Services
{
    public class EmailSendingService
    {
        private readonly IUnitOfWork DataAccess;
        private readonly IMailService _mailService;
        public EmailSendingService(IUnitOfWork unitOfWork, IMailService mailService)
        {
            DataAccess = unitOfWork;
            _mailService = mailService;
        }

        public async void OrderConfirmationEmail(int customerId, int orderId, int amount)
        {
            var customerEmail = DataAccess.Customer.Get(c => c.Id == customerId).Email;
            if(customerEmail == null)
            {
                return;
            }   

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerEmail,
                Subject = "EcomMS: Order Placed",
                Body = $"Your order #{orderId} has been placed with amount {amount} Tk"
            };
            await _mailService.SendEmailAsync(mailRequest);
            return;
        }
    }
}
