using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class RequestTypeSteps
    {
        public RequestTypeSteps()
        {
            ServiceRequests = new HashSet<ServiceRequests>();
            StepHistories = new HashSet<StepHistories>();
        }

        public int Id { get; set; }
        public string StepDescription { get; set; }
        public int StepNumber { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool? Active { get; set; }
        public int RequestType { get; set; }

        public virtual RequestTypes RequestTypeNavigation { get; set; }
        public virtual ICollection<ServiceRequests> ServiceRequests { get; set; }
        public virtual ICollection<StepHistories> StepHistories { get; set; }
    }
}
