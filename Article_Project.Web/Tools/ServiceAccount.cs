using Microsoft.Extensions.Configuration;
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
        public ServiceAccount(IConfiguration configuration)
        {
            _configuration = configuration;
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

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 1000000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            //Use Secrets Manager for Values
            string myEmail = _configuration["Email"];
            string myPassword = _configuration["Password"];

            client.Credentials = new NetworkCredential(myEmail, myPassword);
            MailMessage message = new MailMessage(myEmail, UserEmail, Subject, Body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            client.Send(message);


            return Task.CompletedTask;
        }
    }
}
