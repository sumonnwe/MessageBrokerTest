namespace MQ.Message
{
    public class AlarmMessage : AbstractMessage
    {
        public string AlarmType { get; set; }
        
        public string Priority { get; set; }     

        public AlarmMessage(string deviceId, DateTime timeStamp, string metaData, string sourceSystem, string topic, object message, string alarmType, string priority, string status) : base(deviceId, timeStamp, metaData, sourceSystem, topic, message)
        {
            this.DeviceId = deviceId;
            this.Timestamp = timeStamp;
            this.MetaData = metaData;
            this.SourceSystem = sourceSystem;
            this.Topic = topic;
            this.Message = message;
            this.AlarmType = alarmType;
            this.Priority = priority; 
        }

        public override void DisplayMessageDetails()
        {
            Console.WriteLine($"AlarmMessage - Topic: {Topic}, AlarmType: {AlarmType}, DeviceID: {DeviceId}, Priority: {Priority} ");
        }
    }
}