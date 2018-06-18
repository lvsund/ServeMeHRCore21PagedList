using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class StatusSets
    {
        public StatusSets()
        {
            ServiceRequests = new HashSet<ServiceRequests>();
        }

        public int Id { get; set; }
        public string StatusDescription { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Active { get; set; }
        public int StatusType { get; set; }

        public virtual StatusTypes StatusTypeNavigation { get; set; }
        public virtual  ICollection<ServiceRequests> ServiceRequests { get; set; }
    }
}
