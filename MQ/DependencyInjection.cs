using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Microsoft.Extensions.DependencyInjection;

namespace MQ
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessageServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageProcessor, OrderProcessor>();
            services.AddSingleton<Subscriber>();
            return services;
        }

    }
}
