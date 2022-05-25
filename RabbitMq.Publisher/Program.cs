using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
string message = "Hello World!";
var messageBody = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: messageBody);
Console.WriteLine(" [x] Sent {0}", message);
Console.ReadLine();