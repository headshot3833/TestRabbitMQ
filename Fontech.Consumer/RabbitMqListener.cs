using FonTech.Domain.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Fontech.Consumer;

public class RabbitMqListener : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IOptions<RabbitMqSettings> _options;

    public RabbitMqListener(IOptions<RabbitMqSettings> options)
    {
        _options = options;
        var factory = new ConnectionFactory();
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_options.Value.QeueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (obj, BasicDeliverEventArgs) =>
        {
            var content = Encoding.UTF8.GetString(BasicDeliverEventArgs.Body.ToArray());
            Debug.WriteLine($"Полученно сообщение: {content}");

            _channel.BasicAck(BasicDeliverEventArgs.DeliveryTag, false);
        };
        _channel.BasicConsume(_options.Value.QeueName, false, consumer);

       
        return Task.CompletedTask;
    }
}
