using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQ;
using MQ.DTOs;
using MQ.Handler;
using MQ.Handler.Interfaces;
using RabbitMQ.Client;
using RabbitMQConsumerService;
using System;
using System.Threading.Channels;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Configure RabbitMQ settings
        services.Configure<RabbitMQSettings>(context.Configuration.GetSection("RabbitMQ"));

        // Register the RabbitMQ connection factory
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
            return new ConnectionFactory
            {
                HostName = settings.HostName,
                UserName = settings.UserName,
                Password = settings.Password
            };
        });

        services.AddTransient<IMessageSubscriber<Order>, OrderMessageHandler>();
        services.AddTransient<IMessageSubscriber<User>, UserMessageHandler>();

        // Register RabbitMQ consumer service
        services.AddSingleton<Consumer>();


        var serviceProvider = services.BuildServiceProvider();
        // Start consuming messages 
        
        var consumerService = serviceProvider.GetService<Consumer>();
        consumerService?.StartConsuming("card-access-events-queue");
        consumerService?.StartConsuming("user-command-queue"); 

    })
    .Build(); 
 

await builder.RunAsync();

/*

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Bind RabbitMQ settings
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

// Register RabbitMQ connection factory
builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
    return new ConnectionFactory
    {
        HostName = settings.HostName,
        UserName = settings.UserName,
        Password = settings.Password
    };
});

// Register message handler
builder.Services.AddTransient<IMessageSubscriber<Order>, OrderMessageHandler>();
builder.Services.AddTransient<IMessageSubscriber<User>, UserMessageHandler>();

// Register RabbitMQConsumerService as a hosted service
builder.Services.AddHostedService<RabbitMQConsumerService>();

using IHost host = builder.Build();
  

// Start the host with cancellation token handling
CancellationTokenSource cts = new CancellationTokenSource();
await host.StartAsync(cts.Token);

// Optionally, wait for cancellation or stop conditions
Console.WriteLine("Press Ctrl+C to stop.");
Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

// Stop the host when cancellation is requested
await host.WaitForShutdownAsync();

*/
/*

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Bind RabbitMQ settings
        services.Configure<RabbitMQSettings>(context.Configuration.GetSection("RabbitMQ"));

        // Register RabbitMQ connection factory
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
            return new ConnectionFactory
            {
                HostName = settings.HostName,
                UserName = settings.UserName,
                Password = settings.Password
            };
        });

        // Register message handlers 
        services.AddTransient<IMessageSubscriber<Order>, OrderMessageHandler>();
        services.AddTransient<IMessageSubscriber<User>, UserMessageHandler>();

        // Register RabbitMQ consumer service
        services.AddSingleton<RabbitMQConsumerService>();


        var serviceProvider = services.BuildServiceProvider();

        // Start consuming messages
        var consumerService = serviceProvider.GetService<RabbitMQConsumerService>();
        consumerService?.StartConsuming();

    })
    .Build()
    .Run();

*/
/*

var serviceCollection = new ServiceCollection();

// Register RabbitMQ ConnectionFactory
serviceCollection.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
}); 


// Register message handlers 
serviceCollection.AddTransient<IMessageSubscriber<Order>, OrderMessageHandler>();
serviceCollection.AddTransient<IMessageSubscriber<User>, UserMessageHandler>();

// Register RabbitMQ consumer service
serviceCollection.AddSingleton<RabbitMQConsumerService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

// Start consuming messages
var consumerService = serviceProvider.GetService<RabbitMQConsumerService>();
consumerService?.StartConsuming();

Console.WriteLine("Press [Enter] to exit.");
Console.ReadLine();
 */