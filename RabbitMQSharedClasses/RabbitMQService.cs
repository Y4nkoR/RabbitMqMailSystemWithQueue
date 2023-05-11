using RabbitMQ.Client;
using RabbitMQSharedClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSharedClasses
{
    public class RabbitMQService : IRabbitMQService
    {
        // FOR TESTING PURPOSE
        //private static string USER = "guest";
        //private static string PASSWORD = "guest";
        //private static string V_HOST = "/";
        private static string HOST_NAME = "localhost";   // Shouldn't be declared like this in production
        private static string MAIL_QUEUE_NAME = "mailQueue";

        private IConnection Connection { get; set; }

        public RabbitMQService()
        {
            var connectionFactory = new ConnectionFactory()
            {
                //UserName = USER,
                //Password = PASSWORD,
                //VirtualHost = V_HOST,
                HostName = HOST_NAME
            };

            Connection = connectionFactory.CreateConnection();
        }

        public IConnection GetConnection() { return Connection; }

        public string GetMailQueueName() { return MAIL_QUEUE_NAME; }
    }
}
