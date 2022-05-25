using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

var randomQueueName = channel.QueueDeclare().QueueName;



channel.QueueBind(randomQueueName, "logs-fanout", "", null);
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(randomQueueName, false, consumer);

Console.WriteLine("Waiting for messages...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(" [x] Received {0}", message);
    channel.BasicAck(e.DeliveryTag, false);
};


Console.ReadLine();