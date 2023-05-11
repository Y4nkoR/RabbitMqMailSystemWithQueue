using MailSender;
using RabbitMQSharedClasses.DataObjects;
using System.Net.Mail;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var mailSender = new Sender();

            while (true)
            {
                Console.WriteLine("- Choose option -");
                Console.WriteLine("[1] Send premade mails");
                Console.WriteLine("[2] Create new Smtp mail");
                Console.WriteLine("[q] To exit");

                var option = Console.ReadLine();

                if (option == "1")
                {
                    var mail = new Mail("testSender@onet.pl", new List<string>() { "test@wp.pl" }, "Test subject 1", "<h1> Welcome </h1> This is a <b> test </b> body!", MailType.Smtp);
                    mailSender.Send(mail);

                    mail = new Mail("test@wp.pl", new List<string>() { "joe@gmail.com" }, "Super important", "<h2> Hi Joe! </h2> This is a <b> very </b> <i> important </i> message!", MailType.MailKit);
                    mailSender.Send(mail);

                    Console.Clear();
                    Console.WriteLine("Mails sent!");
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                }
                else if (option == "2")
                {
                    MailAddress sender;
                    Console.Write("Sender's mail: ");
                    while (true)
                    {
            
                        try
                        {
                            sender = new MailAddress(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Mail is not correct.");
                            continue;
                        }

                        break;
                    }

                    var recipients = new List<string>();
                    Console.Write("Recipient's mails (separated by space): ");
                    while (true)
                    {
                        recipients = Console.ReadLine().Split(" ").ToList();
                        try
                        {
                            foreach (var recipient in recipients)
                            {
                                new MailAddress(recipient);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Mail(s) is/are not correct.");
                            continue;
                        }

                        break;
                    }

                    Console.Write("Mail subject: ");
                    var subject = Console.ReadLine();

                    Console.Write("Mail body: ");
                    var body = Console.ReadLine();
       

                    if (subject != null && body != null)
                    {
                        var mail = new Mail(sender.ToString(), recipients, subject, body, MailType.Smtp);

                        mailSender.Send(mail);

                        Console.Clear();
                        Console.WriteLine($"Mail(s) to {recipients.Count} recipient(s) sent!");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Recipient, subject and body can't be empty!");
                    }
        
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                }
                else if (option == "q")
                {
                    break;
                }

                Console.Clear();
            }
        }
    }
}