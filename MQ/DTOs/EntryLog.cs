using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.DTOs
{
    public class EntryLog
    {
        public int EntryLogId { get; set; }
        public DateTime? entryDate { get; set; }
        public int? logIndex { get; set; }
        public int? terminalId { get; set; }

        public int? doorNo { get; set; }
        public int? userId { get; set; }
        public string? cardNo { get; set; }
        public int? eventId { get; set; }
        public string? eventDes { get; set; }
        public int? inoutId { get; set; }
        public string? inoutDes { get; set; }
        public int? verifyId { get; set; }
        public string? verifyDes { get; set; }
        public int? functionKey { get; set; }
    }
}
