using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class StatusTypes
    {
        public StatusTypes()
        {
            StatusSets = new HashSet<StatusSets>();
        }

        public int Id { get; set; }
        public string StatusTypeDescription { get; set; }

        public  virtual ICollection<StatusSets> StatusSets { get; set; }
    }
}
