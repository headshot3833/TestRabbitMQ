using FonTech.Producer.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Producer;

public class Producer : IMessageProducer
{
    public void SendMessage<T>(T message, string rotingKey, string? exchange = null)
    {
        var factory = new ConnectionFactory() {HostName = "localhost"};
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var json = JsonConvert.SerializeObject(message, Formatting.Indented,
        new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        });

        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange, rotingKey, body: body);
    }
}
