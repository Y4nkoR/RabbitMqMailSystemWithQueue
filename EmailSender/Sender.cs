using RabbitMQ.Client;
using RabbitMQSharedClasses;
using RabbitMQSharedClasses.DataObjects;
using RabbitMQSharedClasses.Interfaces;
using System.Text;
using System.Text.Json;

namespace MailSender
{
    public class Sender
    {
        private string QueueName { get; set; }
        private IModel Channel { get; set; }

        public Sender() 
        {
            IRabbitMQService rabbitService = new RabbitMQService();
            var connection = rabbitService.GetConnection();
            QueueName = rabbitService.GetMailQueueName();

            Channel = connection.CreateModel();
            Channel.QueueDeclare(queue: QueueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        }

        public void Send(Mail mail)
        {
            var serializedMail = JsonSerializer.Serialize(mail);

            Channel.BasicPublish(exchange: "",
                routingKey: QueueName,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(serializedMail));
        }
    }
}