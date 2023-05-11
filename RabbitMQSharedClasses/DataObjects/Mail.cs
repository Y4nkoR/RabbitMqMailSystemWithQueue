using System.Net;
using System.Net.Mail;

namespace RabbitMQSharedClasses.DataObjects
{
    public enum MailType
    {
        Smtp,
        MailKit
    }

    public class Mail
    {
        public string Sender { get; private set; }
        public List<string> Recipients { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public MailType MailType { get; private set; }

        public Mail(string sender, List<string> recipients, string subject, string body, MailType mailType)
        {
            Sender = sender;
            Recipients = recipients;
            Subject = subject;
            Body = body;
            MailType = mailType;
        }

        public void SendSmtp()
        {
            var smtpClient = new SmtpClient("smtp.ethereal.email")
            {
                Port = 587,
                Credentials = new NetworkCredential("andre83@ethereal.email", "jrUxDJvfkkSkH8gDck"),     // In practice, it should not be stored here.
                EnableSsl = true,
            };

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Sender);

            foreach (var recipient in Recipients)
            {
                mailMessage.To.Add(new MailAddress(recipient));

            }

            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;

            smtpClient.Send(mailMessage);
        }

        public void SendMailKit()
        {
            // Implement here..
        }
    }
}
