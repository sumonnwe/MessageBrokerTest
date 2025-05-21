namespace MQ.Message
{
    public class EventMessage : AbstractMessage
    {
        public string EventType { get; set; } 

        public EventMessage(string deviceId, DateTime timeStamp, string metaData, string sourceSystem, string topic, object message, string eventType) : base(deviceId, timeStamp, metaData, sourceSystem, topic, message)
        {
            this.DeviceId = deviceId;
            this.Timestamp = timeStamp;
            this.MetaData = metaData;
            this.SourceSystem = sourceSystem;
            this.Topic = topic;
            this.Message = message;
            this.EventType = eventType;
        }

        public override void DisplayMessageDetails()
        {
            Console.WriteLine($"EventMessage- Topic: {Topic}, EventType: {EventType}, DeviceID: {DeviceId} ");
        }
    }
}