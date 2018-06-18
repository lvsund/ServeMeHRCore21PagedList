using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class Members
    {
        public Members()
        {
            IndividualAssignmentHistories = new HashSet<IndividualAssignmentHistories>();
            ServiceRequests = new HashSet<ServiceRequests>();
            TeamMembers = new HashSet<TeamMembers>();
        }

        public int Id { get; set; }
        public string MemberUserid { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFullName { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhone { get; set; }

        public virtual ICollection<IndividualAssignmentHistories> IndividualAssignmentHistories { get; set; }
        public virtual ICollection<ServiceRequests> ServiceRequests { get; set; }
        public  virtual ICollection<TeamMembers> TeamMembers { get; set; }
    }
}
