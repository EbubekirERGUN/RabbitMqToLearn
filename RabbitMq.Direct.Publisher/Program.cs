using System.Text;
using RabbitMq.Direct.Publisher;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Fanout);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(i =>
{
    var routeKey = $"route-key-{i}";
    var queueName = $"Direct-queue-{i}";
    channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueBind(queue: queueName,
                      exchange: "logs-direct",
                      routingKey: routeKey);
});


Enumerable.Range(1, 50).ToList().ForEach(i =>
{
    LogNames log = (LogNames)new Random().Next(1, 5);
    var message = $"Log-type {log}";
    var body = Encoding.UTF8.GetBytes(message);
    var routeKey = $"route-key-{log}";

    channel.BasicPublish(exchange: "logs-direct",
                         routingKey: routeKey,
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Log Sent {message}");
});
