using RabbitMQ.Client;
using System.Text;

namespace SequenceWebApi.Processing
{
    public class Publisher
    {
        public void PublishTaskMessage<T>(T message)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672//,
                //UserName = "guest",
                //Password = "guest",
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using
            var channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("tasks", exclusive: false);
            //Serialize the message
            var json = System.Text.Json.JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            channel.BasicPublish(exchange: "", routingKey: "tasks", body: body);
        }
    }
}
