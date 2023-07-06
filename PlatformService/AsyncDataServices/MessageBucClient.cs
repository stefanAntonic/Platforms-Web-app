using System.Text;
using System.Text.Json;
using PlatformService.Dto;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBucClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBucClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"]),

        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }
        catch (Exception e)
        {
            Console.WriteLine($"---> Unable to connect to Message bus, error : {e} ");
            throw;
        }

        {

        }
    }

    public async void PublishNewPlatform(PlatformPublishDto platformPublishDto)
    {
        var message = JsonSerializer.Serialize(platformPublishDto);
        if (_connection.IsOpen)
        {
            Console.WriteLine("---> RabbitMQ Connection Open, sending message...");
            SendMessage(message);

        }
        else
        {
            Console.WriteLine("---> RabbitMQ Connection Closed, not sending");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body);
    }

    public void Clean()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs eventArgs)
    {
            Console.WriteLine("---> RabbitMQ Connection Shutdown");
    }
}