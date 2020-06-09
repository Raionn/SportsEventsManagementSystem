using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Net;

namespace SportBook.Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;
        private string _host;
        private string _from;
        public EmailHelper(IConfiguration iConfiguration)
        {
            _config = iConfiguration;
            var smtpSection = iConfiguration.GetSection("SMTP");
            if (smtpSection != null)
            {
                _host = smtpSection.GetSection("Host").Value;
                _from = smtpSection.GetSection("From").Value;
            }
        }

        public Task SendEmail(EmailModel emailModel)
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_from)
                };

                // The important part -- configuring the SMTP client
                SmtpClient smtp = new SmtpClient
                {
                    Port = 587,   // [1] You can try with 465 also, I always used 587 and got success
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network, // [2] Added this
                    UseDefaultCredentials = false, // [3] Changed this
                    Credentials = new NetworkCredential(mail.From.ToString(), _config.GetValue<string>("ConnectionStrings:Mail_Password")),  // [4] Added this. Note, first parameter is NOT string.
                    Host = _host
                };
                mail.IsBodyHtml = true;
                //recipient address
                mail.To.Add(new MailAddress(emailModel.To));
                mail.Subject = emailModel.Subject;
                //Formatted mail body
                mail.IsBodyHtml = true;
                mail.Body = emailModel.Message;
                smtp.Send(mail);
            }
            catch (Exception)
            {
            }
            return Task.CompletedTask;
        }
    }

    public class EmailModel
    {
        public EmailModel(string to, string subject, string message)
        {
            To = to;
            Subject = subject;
            Message = message;
        }
        public string To
        {
            get;
        }
        public string Subject
        {
            get;
        }
        public string Message
        {
            get;
        }
    }
}
