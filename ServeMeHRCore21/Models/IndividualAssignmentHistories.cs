using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class IndividualAssignmentHistories
    {
        public int Id { get; set; }
        public int AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime DateAssigned { get; set; }
        public int? ServiceRequest { get; set; }

        public virtual Members AssignedToNavigation { get; set; }
        public virtual ServiceRequests ServiceRequestNavigation { get; set; }
    }
}
