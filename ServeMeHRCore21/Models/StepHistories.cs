using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class StepHistories
    {
        public int Id { get; set; }
        public DateTime LastUpdated { get; set; }
        public int RequestTypeStep { get; set; }
        public int? ServiceRequest { get; set; }

        public virtual RequestTypeSteps RequestTypeStepNavigation { get; set; }
        public virtual ServiceRequests ServiceRequestNavigation { get; set; }
    }
}
