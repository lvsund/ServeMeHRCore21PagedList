using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class TeamMembers
    {
        public int Id { get; set; }
        public int Member { get; set; }
        public int Team { get; set; }

        public virtual Members MemberNavigation { get; set; }
        public virtual  Teams TeamNavigation { get; set; }
    }
}
