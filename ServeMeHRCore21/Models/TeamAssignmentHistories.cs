using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class TeamAssignmentHistories
    {
        public int Id { get; set; }
        public string AssignedBy { get; set; }
        public DateTime DateAssigned { get; set; }
        public int ServiceRequest { get; set; }
        public int Team { get; set; }

        public  virtual ServiceRequests ServiceRequestNavigation { get; set; }
        public  virtual Teams TeamNavigation { get; set; }
    }
}
