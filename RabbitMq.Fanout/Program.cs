using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);
Enumerable.Range(1, 50).ToList().ForEach(i =>
{
    var message = $"Log {i}";
    var body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: "logs-fanout",
                         routingKey: "",
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Sent {message}");
});
