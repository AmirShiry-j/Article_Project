using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Web.Tools
{
    public interface IServiceAccount
    {
        void DeleteImageProfile(string ImageProfileName);

        Task SendEmail(string UserEmail, string Body, string Subject);

    }
    public class ServiceAccount : IServiceAccount
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ServiceAccount> _logger;
        public ServiceAccount(IConfiguration configuration,
            ILogger<ServiceAccount> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public void DeleteImageProfile(string ImageProfileName)
        {
            string PathImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ImgProfile/", ImageProfileName);
            if (System.IO.File.Exists(PathImage))
            {
                System.IO.File.Delete(PathImage);
            }
        }
        public Task SendEmail(string UserEmail, string Body, string Subject)
        {
            //enable less secure apps in account google with link
            //https://myaccount.google.com/lesssecureapps


            //https://mail.google.com/mail/u/0/?tab=km#inbox

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.UseDefaultCredentials = false;

                //Use Secrets Manager for Values
                string myEmail = "PazelShop09@gmail.com";
                string myPassword = "mkzsra5943";

                client.Credentials = new NetworkCredential(myEmail, myPassword);
                MailMessage message = new MailMessage(myEmail, UserEmail, Subject, Body);
                message.IsBodyHtml = true;
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                client.Send(message);

                return Task.CompletedTask;

            }
            catch (Exception exe)
            {
                _logger.LogError(exe.ToString());

                return Task.CompletedTask;
            }
        }
    }
}
