using System;


namespace MQ.Message
{

    public abstract class AbstractMessage
    {

        public string DeviceId { get; set; } 
        public DateTime Timestamp { get; set; }
        public string MetaData { get; set; }
        public string SourceSystem { get; set; }
        public string Topic { get; set; }
        public object Message { get; set; }

        public AbstractMessage(string deviceId, DateTime timeStamp, string metaData, string sourceSystem, string topic, object message)
        {
            this.DeviceId = deviceId;
            this.Timestamp = timeStamp;
            this.MetaData = metaData;
            this.SourceSystem = sourceSystem;
            this.Topic = topic;
            this.Message = message;
        } 

        public abstract void DisplayMessageDetails();
    }
}