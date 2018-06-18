using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class RequestTypes
    {
        public RequestTypes()
        {
            RequestTypeSteps = new HashSet<RequestTypeSteps>();
            ServiceRequests = new HashSet<ServiceRequests>();
        }

        public int Id { get; set; }
        public string RequestTypeDescription { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool? Active { get; set; }
        public int Team { get; set; }

        public virtual Teams TeamNavigation { get; set; }
        public virtual ICollection<RequestTypeSteps> RequestTypeSteps { get; set; }
        public virtual ICollection<ServiceRequests> ServiceRequests { get; set; }
    }
}
