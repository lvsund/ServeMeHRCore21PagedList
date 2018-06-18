using System;
using System.Collections.Generic;

namespace ServeMeHRCore21.Models
{
    public partial class ApplicConfs
    {
        public int Id { get; set; }
        public bool FileSystemUpload { get; set; }
        public bool Adactive { get; set; }
        public bool EmailConfirmation { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public string AppAdmin { get; set; }
        public string BackAdmin { get; set; }
        public string Ldapconn { get; set; }
        public string Ldappath { get; set; }
        public string ManageHremail { get; set; }
        public string ManageHremailPass { get; set; }
        public string Smtphost { get; set; }
        public int? Smtpport { get; set; }
        public bool? EnableSsl { get; set; }
    }
}
