using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQSharedClasses;
using RabbitMQSharedClasses.Interfaces;
using RabbitMQSharedClasses.DataObjects;
using System.Net.Mail;
using System.Net;
using System;

namespace MailReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            IRabbitMQService rabbitMQService = new RabbitMQService();
            var connection = rabbitMQService.GetConnection();
            var queueName = rabbitMQService.GetMailQueueName();

            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var mail = JsonSerializer.Deserialize<Mail>(Encoding.UTF8.GetString(body));

                    if (mail == null)
                    {
                        throw new ArgumentException("Received data was not an mail!");
                    }

                    var recipients = String.Join(", ", mail.Recipients);
                    Console.WriteLine($"[{mail.MailType}] From: {mail.Sender}, To: {recipients}, Subject: {mail.Subject}, Body: {mail.Body}");

                    switch (mail.MailType)
                    {
                        case MailType.Smtp:
                            mail.SendSmtp();
                            break;
                        case MailType.MailKit:
                            mail.SendMailKit();
                            break;
                        default:
                            throw new ArgumentException("Unknown mail type!");
                    }

                    channel.BasicAck(ea.DeliveryTag, true);
                    try
                    {
                        
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Receiving data failed!", ex);
                    }                    
                };

                channel.BasicConsume(queue: queueName,
                    autoAck: false,
                    consumer: consumer);
                Console.WriteLine("Waiting for mails...");
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}