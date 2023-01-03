using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Receive
{
    public static string GetTransformedText(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        return string.Empty;
    }

    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(
                queue: "queueLine",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Received {message} and transformed to -> {GetTransformedText(message)}");
            };

            channel.BasicConsume(
                queue: "queueLine",
                autoAck: true,
                consumer: consumer
            );

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}