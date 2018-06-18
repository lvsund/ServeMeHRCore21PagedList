using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class ServiceRequestNotes
    {
        public int Id { get; set; }
        public string NoteDescription { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string WrittenBy { get; set; }
        public int? ServiceRequest { get; set; }

        public virtual ServiceRequests ServiceRequestNavigation { get; set; }
    }
}
