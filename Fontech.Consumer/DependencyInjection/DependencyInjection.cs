using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontech.Consumer.DependencyInjection;

public static class DependencyInjection
{
    public static void addConsumer(this IServiceCollection service)
    {
        service.AddHostedService<RabbitMqListener>();
    }
}
