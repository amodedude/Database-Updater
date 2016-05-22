using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace RTI.Database.Updater
{
    /// <summary>
    /// Sends email messages to recipients. 
    /// </summary>
    internal class EmailService
    {
        /// <summary>
        /// Sends an email alert to the user via the SmtpClient.
        /// </summary>
        /// <param name="adressList"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        internal void SendMail(List<string> addressList, string subject, string body)
        {
            try
            {
                if(addressList == null || addressList.Count == 0)
                    throw new ArgumentException("Bad Recipients List");

                using (var client = new SmtpClient())
                {
                    client.Host = "smtp.Gmail.com";
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("rti.notificationservice", "R#cir720!");

                    using (var message = new MailMessage())
                    {
                        message.Body = body;
                        message.Subject = subject;
                        message.From = new MailAddress("rti.notificationservice@gmail.com");

                        foreach (var recipient in addressList)
                            message.To.Add(recipient);

                        client.Send(message);
                    }
                }
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
