using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
var queueName = "Direct-queue-Critical";
channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Waiting for messages...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1000);
    Console.WriteLine(" [x] Received {0}", message);
    File.AppendAllText("log-critical.txt", $" [x] Received {message}");
    channel.BasicAck(e.DeliveryTag, false);
};


Console.ReadLine();