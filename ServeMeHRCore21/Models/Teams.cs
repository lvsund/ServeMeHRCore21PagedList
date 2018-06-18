using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class Teams
    {
        public Teams()
        {
            Priorities = new HashSet<Priorities>();
            RequestTypes = new HashSet<RequestTypes>();
            ServiceRequests = new HashSet<ServiceRequests>();
            TeamAssignmentHistories = new HashSet<TeamAssignmentHistories>();
            TeamMembers = new HashSet<TeamMembers>();
        }

        public int Id { get; set; }
        public string TeamDescription { get; set; }
        public string TeamEmailAddress { get; set; }

        public virtual ICollection<Priorities> Priorities { get; set; }
        public  virtual ICollection<RequestTypes> RequestTypes { get; set; }
        public virtual ICollection<ServiceRequests> ServiceRequests { get; set; }
        public  virtual ICollection<TeamAssignmentHistories> TeamAssignmentHistories { get; set; }
        public virtual ICollection<TeamMembers> TeamMembers { get; set; }
    }
}
