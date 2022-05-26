using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "any");

channel.QueueBind(queueName, "header-exchange", String.Empty, headers);
channel.BasicConsume(queueName, false, consumer);
Console.WriteLine("Waiting for messages...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(" [x] Received {0}", message);
    channel.BasicAck(e.DeliveryTag, false);
};


Console.ReadLine();