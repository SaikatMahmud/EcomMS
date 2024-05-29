using EcomMS.DAL.UnitOfWork;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.SendEmail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IUnitOfWork DataAccess;
        public MailService(IUnitOfWork unitOfWork, IOptions<MailSettings> mailSettings)
        {
            DataAccess = unitOfWork;
            _mailSettings = mailSettings.Value;
        }

        public bool OrderConfirmationEmail(int customerId, int orderId, int amount)
        {
            var customerEmail = DataAccess.Customer.Get(c => c.Id == customerId).Email;
            if (customerEmail == null)
            {
                return false;
            }

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerEmail,
                Subject = "EcomMS: Order Placed Notification",
                Body = $"Your order <b>#{orderId}</b> has been placed with amount <b>{amount}</b> Tk."
            };
            Task.Run(async () => await SendEmailAsync(mailRequest));
            return true;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                //if (mailRequest.Attachments != null)
                //{
                //    byte[] fileBytes;
                //    foreach (var file in mailRequest.Attachments)
                //    {
                //        if (file.Length > 0)
                //        {
                //            using (var ms = new MemoryStream())
                //            {
                //                file.CopyTo(ms);
                //                fileBytes = ms.ToArray();
                //            }
                //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                //        }
                //    }
                //}
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
