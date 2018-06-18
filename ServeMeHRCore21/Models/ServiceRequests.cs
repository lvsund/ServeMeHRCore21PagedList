using System;
using System.Collections.Generic;
//using System.ComponentModel;

namespace ServeMeHRCore21.Models
{
    public partial class ServiceRequests
    {
        public ServiceRequests()
        {
            FileDetails = new HashSet<FileDetails>();
            IndividualAssignmentHistories = new HashSet<IndividualAssignmentHistories>();
            ServiceRequestNotes = new HashSet<ServiceRequestNotes>();
            StepHistories = new HashSet<StepHistories>();
            TeamAssignmentHistories = new HashSet<TeamAssignmentHistories>();
        }


      
        //[ReadOnly(true)]
        public int Id { get; set; }
        public string RequestHeading { get; set; }
        public string RequestDescription { get; set; }
        public string RequestorId { get; set; }
        public string RequestorFirstName { get; set; }
        public string RequestorLastName { get; set; }
        public string RequestorPhone { get; set; }
        public string RequestorEmail { get; set; }
        public DateTime? DateTimeSubmitted { get; set; }
        public DateTime? DateTimeStarted { get; set; }
        public DateTime? DateTimeCompleted { get; set; }
        public int? Priority { get; set; }
        public int? RequestType { get; set; }
        public int? RequestTypeStep { get; set; }
        public int? Member { get; set; }
        public int Status { get; set; }
        public int Team { get; set; }

        public virtual Members MemberNavigation { get; set; }
        public virtual Priorities PriorityNavigation { get; set; }
        public virtual RequestTypes RequestTypeNavigation { get; set; }
        public virtual RequestTypeSteps RequestTypeStepNavigation { get; set; }
        public virtual StatusSets StatusNavigation { get; set; }
        public virtual Teams TeamNavigation { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public virtual ICollection<IndividualAssignmentHistories> IndividualAssignmentHistories { get; set; }
        public virtual ICollection<ServiceRequestNotes> ServiceRequestNotes { get; set; }
        public virtual ICollection<StepHistories> StepHistories { get; set; }
        public virtual ICollection<TeamAssignmentHistories> TeamAssignmentHistories { get; set; }
    }
}
