using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ;
using MQ.Handler.Interfaces; 
using MQ.DTOs;
using MQ.Message;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace RabbitMQPublisherService
{
    public  class Publisher 
    {
        private readonly IServiceProvider _serviceProvider;

        public Publisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void PublishEvent()
        {  
            var myService = _serviceProvider.GetRequiredService<IMessagePublisher>();
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
            Console.WriteLine(" [x] Sending request...");
            string response = await myService.PublishMessage(JsonConvert.SerializeObject(eventMessage), "card-event-queue", "card-event-exchange", "card.event");

            if (string.IsNullOrEmpty(response))
            {
                // Handle case where no response is received
                Console.WriteLine(" [!] No response received from the service.");
                return; // Exit the method or take appropriate action (e.g., retry or log)
            }

            try
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(response);
                Console.WriteLine($" [.] Received Response: {response}");
                Console.WriteLine($" [.] Status: {jsonObject.status}");
                Console.WriteLine($" [.] Message: {jsonObject.msg}");
            }
            catch (Exception ex)
            {
                // Handle invalid JSON response
                Console.WriteLine($" [!] Error deserializing response: {ex.Message}");
            }

        }

        public async void PublishUser()
        {
            var myService = _serviceProvider.GetRequiredService<IMessagePublisher>();
            //1. Add User from AppCore repository publish to MessageBroker (- MessageBroker - DeviceService - SEMAC) 

            var Data = new ControllerMessage<User>()
            {
                terminalSetup = "http://localhost:8001/",
                serialNo = "02f873",
                Model = new User()
                {
                    userId = 11, //neglect
                    userName = "User11",
                    isEnabled = true,
                    personalPassword = "",
                    cardNo = "111111",
                    userType = 0,
                    checkExpiration = false,
                    expireFrom = DateTime.UtcNow,
                    expireTo = DateTime.Now.AddDays(2),
                    bypassTimezoneLevel = 1,
                    groupList = [1],
                    doorList = [new Door() { doorNo = 1, access = true, timezoneId = 1 }]
                }
            };
             
            //string entryLog = JsonConvert.SerializeObject(entryLogObject);
            //Prepare Message to Publish
            var commandMessage = new CommandMessage(
                deviceId: "Device001",
                timeStamp: DateTime.Now,
                metaData: "CommandMetaData",
                sourceSystem: "SEMAC Controller",
                topic: "UserRegister",
                message: Data,
                commandType: "Request to Controller", //OPEN, CLOSE, START, STOP
                priority: "", //high, medium, low
                status: "" //Used to acknowledge whether the command was successfully executed (included in the response message).
            );

            Console.WriteLine(" [x] Sending request...");
            string response = await myService.PublishMessage(JsonConvert.SerializeObject(commandMessage), "command-control-queue", "command-control-exchange", "user.controls");

            if (string.IsNullOrEmpty(response))
            {
                // Handle case where no response is received
                Console.WriteLine(" [!] No response received from the service.");
                return; // Exit the method or take appropriate action (e.g., retry or log)
            }

            try
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(response);
                Console.WriteLine($" [.] Received Response: {response}");
                Console.WriteLine($" [.] Status: {jsonObject.status}");
                Console.WriteLine($" [.] Message: {jsonObject.msg}");
            }
            catch (Exception ex)
            {
                // Handle invalid JSON response
                Console.WriteLine($" [!] Error deserializing response: {ex.Message}");
            } 

        }

        public async void PublishDoor()
        {
            var myService = _serviceProvider.GetRequiredService<IMessagePublisher>();
            //1. Add User from AppCore repository publish to MessageBroker (- MessageBroker - DeviceService - SEMAC) 

            var Data = new ControllerMessage<Door>()
            {
                terminalSetup = "http://localhost:8001/",
                serialNo = "07e236",
                Model = new Door()
                {
                    doorNo = 1, 
                    relayType = 0 //0:Normal Open；1:Force Open；2:Force Close；3:Back to Normal State
                }
            };

            //string entryLog = JsonConvert.SerializeObject(entryLogObject);
            //Prepare Message to Publish
            var commandMessage = new CommandMessage(
                deviceId: "Device001",
                timeStamp: DateTime.Now,
                metaData: "CommandMetaData",
                sourceSystem: "SEMAC Controller",
                topic: "DoorOpen",
                message: Data,
                commandType: "Request to Controller", //OPEN, CLOSE, START, STOP
                priority: "", //high, medium, low
                status: "" //Used to acknowledge whether the command was successfully executed (included in the response message).
            );
            string response = await myService.PublishMessage(JsonConvert.SerializeObject(commandMessage), "gate-control-queue", "gate-control-exchange", "gate.controls");

            if (string.IsNullOrEmpty(response))
            {
                // Handle case where no response is received
                Console.WriteLine(" [!] No response received from the service.");
                return; // Exit the method or take appropriate action (e.g., retry or log)
            }

            try
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(response);
                Console.WriteLine($" [.] Received Response: {response}");
                Console.WriteLine($" [.] Status: {jsonObject.status}");
                Console.WriteLine($" [.] Message: {jsonObject.msg}");
            }
            catch (Exception ex)
            {
                // Handle invalid JSON response
                Console.WriteLine($" [!] Error deserializing response: {ex.Message}");
            }
        }

        public async Task<string> PublishMessageWithResponse(string message, string queueName, string exchangeName, string routingKey)
        {

            //Callback replyQ
            var correlationId = Guid.NewGuid().ToString();
            var props = _channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = correlationId;
            //Set the message
            var body = Encoding.UTF8.GetBytes(message);

            // Declare the exchange
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
            //If just want to declare exchange, no need following two lines to bind to queue.
            //_channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            //_channel.QueueBind(queueName, exchangeName, routingKey);


            // Publish the message to the exchange
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: props, body: body);

            Console.WriteLine($"[X] Published message: {message}");
            return await _responseTask.Task;

            //_responseDict[correlationId] = null;
            //while (_responseDict[correlationId] == null)
            //{
            //Waiting for response
            //}

            //return _responseDict[correlationId]; 
        }


    }
}
