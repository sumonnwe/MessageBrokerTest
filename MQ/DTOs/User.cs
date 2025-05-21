namespace MQ.DTOs
{
    public class User
    {
        public User()
        {
            userId = 1;
            employeeId = string.Empty;
            userName = string.Empty;
            isEnabled = true;
            personalPassword = string.Empty;
            cardNo = string.Empty;
            userType = 1;
            checkExpiration = false;
            expireFrom = DateTime.MinValue;
            expireTo = DateTime.MinValue;
            bypassTimezoneLevel = 0;
            fpSize = 0;
            fpBuffer1 = string.Empty;
            fpBuffer2 = string.Empty;
            faceBuffer = string.Empty;
            groupList = new List<int>();
            doorList = new List<Door>();
        }
        public int userId { get; set; }
        public string? employeeId { get; set; }
        public string? userName { get; set; }
        public bool? isEnabled { get; set; }

        public string? personalPassword { get; set; }
        public string? cardNo { get; set; }
        public int? userType { get; set; }
        public bool? checkExpiration { get; set; }
        public DateTime? expireFrom { get; set; }
        public DateTime? expireTo { get; set; }
        public int? bypassTimezoneLevel { get; set; }

        public int? fpSize { get; set; }
        public string? fpBuffer1 { get; set; }
        public string? fpBuffer2 { get; set; }

        public string? faceBuffer { get; set; }

        public List<int>? groupList { get; set; }
        public List<Door>? doorList { get; set; }

    }
     
}

 