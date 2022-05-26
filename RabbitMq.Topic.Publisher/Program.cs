using System.Text;
using RabbitMq.Topic.Publisher;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-header", durable: true, type: ExchangeType.Topic);

Random rnd = new Random();

Enumerable.Range(1, 50).ToList().ForEach(i =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";
    string message = $"log-type: {log1}-{log2}-{log3}";
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "logs-header",
                         routingKey: routeKey,
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Log Sent {message}");
});
