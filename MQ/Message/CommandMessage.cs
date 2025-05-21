namespace MQ.Message
{
    public class CommandMessage : AbstractMessage
    {
        public string CommandType { get; set; }
        
        public string Priority { get; set; }    

        public string Status { get; set; }

        //public ControllerMessage Controller { set; get; }
        public CommandMessage(string deviceId, DateTime timeStamp, string metaData, string sourceSystem, string topic, object message, string commandType, string priority, string status) : base(deviceId, timeStamp, metaData, sourceSystem, topic, message)
        {

            this.DeviceId = deviceId;
            this.Timestamp = timeStamp;
            this.MetaData = metaData;
            this.SourceSystem = sourceSystem;
            this.Topic = topic;
            this.Message = message;
            this.CommandType = commandType;
            this.Priority = priority;
            this.Status = status; 
        }

        public override void DisplayMessageDetails()
        {
            Console.WriteLine($"CommandMessage- Topic: {Topic}, CommandType: {CommandType}, DeviceID: {DeviceId}, Priority: {Priority}, Status: {Status} ");
        }
    }
}