using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQ;
using RabbitMQ.Client;
using Newtonsoft.Json;
using MQ.Handler.Interfaces;
using MQ.DTOs;
using MQ.Message;
using RabbitMQPublisherService;

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

        // Register the RabbitMQPublisher as a service
        services.AddScoped<IMessagePublisher, RabbitMQPublisher>();

        services.AddSingleton<Publisher>();
        var serviceProvider = services.BuildServiceProvider();
        var publisherService = serviceProvider.GetService<Publisher>();
        //publisherService.PublishEvent();
        publisherService.PublishUser();
        //publisherService.PublishDoor();

    })
    .Build();
 


/*
using (var scope = builder.Services.CreateScope())
{
    Publisher p = new Publisher(scope.ServiceProvider);
    p.PublishEvent();
    p.PublishUser();
}

// Use the publisher to publish a test message
using (var scope = builder.Services.CreateScope())
{
    var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();  

    //1. Add User from AppCore repository publish to MessageBroker (- MessageBroker - DeviceService - SEMAC)
    var userObject = new User
    {          
        userId = 1,
          userName = "Kobe",
          isEnabled = true,
          personalPassword = "",
          cardNo = "12345678",
          userType = 0,
          checkExpiration = false,
          expireFrom = DateTime.UtcNow,
          expireTo = DateTime.Now.AddDays(2),
          bypassTimezoneLevel = 1,
          groupList = [1],
          doorList = [ new Door () { doorNo = 1,  access =  true, timezoneId = 1  } ]
    };
    //string entryLog = JsonConvert.SerializeObject(entryLogObject);
    //Prepare Message to Publish
    var commandMessage = new CommandMessage(
        deviceId: "Device001",
        timeStamp: DateTime.Now,
        metaData: "CommandMetaData",
        sourceSystem: "SEMAC Controller",
        topic: "UserRegister",
        message: userObject,
        commandType: "Request to Controller", //OPEN, CLOSE, START, STOP
        priority: "", //high, medium, low
        status: "" //Used to acknowledge whether the command was successfully executed (included in the response message).
    );
    publisher.PublishMessage(JsonConvert.SerializeObject(commandMessage), "commands-exchange", "user.commands");


    //2. retrieve data once trigger swipe card
    var entryLogObject = new EntryLog
    {
        EntryLogId = 0,
        entryDate = DateTime.Now,
        logIndex = 1,
        terminalId = 1,
        doorNo = 1,
        userId = 10,
        cardNo = "111111",
        eventId = 1,
        eventDes = "event des",
        inoutId = 1,
        inoutDes = "Inout Des",
        verifyId = 1,
        verifyDes = "verifyDes",
        functionKey = 1
    };
    //string entryLog = JsonConvert.SerializeObject(entryLogObject);
    //Prepare Message to Publish
    var eventMessage = new EventMessage(
        deviceId: "Device001",
        timeStamp: DateTime.Now,
        metaData: "EventMetaData",
        sourceSystem: "SEMAC Controller",
        topic: "CardTransaction",
        message: entryLogObject,
        eventType: "REALTIME_LOG" //ACCESS_GRANTED, DEVICE_STARTED, TEMP_THRESHOLD_EXCEEDED
    ); 
    publisher.PublishMessage(JsonConvert.SerializeObject(eventMessage), "event-exchange", "card.events"); 
      
}
*/

await builder.RunAsync();


/*
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQ;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Nodes;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register IMessagePublisher and its implementation
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Register the ConsoleApp class to drive the application
        services.AddTransient<PublishService>();
    })
    .Build();

// Resolve PublishService and run the main logic
var app = host.Services.GetRequiredService<PublishService>();

var order = new MQ.Order
{
    OrderId = 12345,
    Description = "Order Description ",
    CustomerName = "John Doe",
    Amount = 99.99m,
    OrderDate = DateTime.Now
};
//string message = "Message From Program";
string message = JsonConvert.SerializeObject(order); 
await app.RunAsync(message);
*/

/*
using MQ;
using RabbitMQPublisher;
using RabbitMQPublisherService;

class Program
{
    static void Main(string[] args)
    {
        var publisher = new Publisher();

        // Example order object
        var order = new MQ.Order
        {
            OrderId = 123,
            Description = "Order Description ",
            CustomerName = "John Doe",
            Amount = 99.99m,
            OrderDate = DateTime.Now
        };

        // Publish the order object
        publisher.PublishOrder(order, "order_exchange", "order_queue", "order_routing_key");

        ///var orderHandler = new OrderHandler();
        //var subscriber = new Subscriber();
        //subscriber.StartListening("orders_exchange");
        //subscriber.StartListening(orderHandler.ProcessOrderMessage, "orders_exchange");
    }
} */