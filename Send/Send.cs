using RabbitMQ.Client;
using System.Text;

class Send
{
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

            int count = 10;
            while (count != 0)
            {
                string message = $"Hello World => {count}!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "queueLine",
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine($" [x] Sent {message}");

                count -= 1;
                Thread.Sleep(5000);
            }
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}