namespace MQ.DTOs
{
    public class Door
    {
        public Door()
        {
            doorNo = 1;
            access = false;
            time = 0;
            timezoneId = 0;
            relayType = 0;
        }

        public int? doorNo { get; set; }
        public bool? access { get; set; }
        public int? timezoneId { get; set; }
        public int? time { get; set; }
        public int? relayType { get; set; }

    }
}
