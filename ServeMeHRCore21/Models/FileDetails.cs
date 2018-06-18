using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class FileDetails
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int? ServiceRequestId { get; set; }

        public virtual ServiceRequests ServiceRequest { get; set; }
    }
}
