using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class Priorities
    {
        public Priorities()
        {
            ServiceRequests = new HashSet<ServiceRequests>();
        }

        public int Id { get; set; }
        public string PriorityDescription { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Active { get; set; }
        public int Team { get; set; }

        public virtual Teams TeamNavigation { get; set; }
        public virtual ICollection<ServiceRequests> ServiceRequests { get; set; }
    }
}
