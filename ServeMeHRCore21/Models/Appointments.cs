using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class Appointments
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string MsgId { get; set; }
        public int? MsgSequence { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
    }
}
