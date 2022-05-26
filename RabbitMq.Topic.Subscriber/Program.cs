using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
var QueueName = channel.QueueDeclare().QueueName;
var routeKey = "*.Error.*";
channel.QueueBind(QueueName, "logs-header", routeKey);
channel.BasicConsume(QueueName, false, consumer);
Console.WriteLine("Waiting for messages...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(" [x] Received {0}", message);
    channel.BasicAck(e.DeliveryTag, false);
};


Console.ReadLine();