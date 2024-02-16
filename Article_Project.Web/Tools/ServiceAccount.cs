using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<EmailSetting> _emailSetting;
        private readonly ILogger<ServiceAccount> _logger;
        public ServiceAccount(IOptions<EmailSetting> emailSetting,
            ILogger<ServiceAccount> logger)
        {
            _emailSetting = emailSetting;
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
                client.Port = _emailSetting.Value.Port;
                client.Host = _emailSetting.Value.Host;
                client.EnableSsl = _emailSetting.Value.EnableSsl;
                client.Timeout = _emailSetting.Value.Timeout;
                client.UseDefaultCredentials = _emailSetting.Value.UseDefaultCredentials;

                //Use Secrets Manager for Values
                string myEmail = _emailSetting.Value.Email;
                string myPassword = _emailSetting.Value.Password;

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
