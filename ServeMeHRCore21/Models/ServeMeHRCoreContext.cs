using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ServeMeHRCore21.Models
{
    public partial class ServeMeHRCoreContext : DbContext
    {
        public virtual DbSet<Adinformations> Adinformations { get; set; }
        public virtual DbSet<ApplicConfs> ApplicConfs { get; set; }
        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<FileDetails> FileDetails { get; set; }
        public virtual DbSet<IndividualAssignmentHistories> IndividualAssignmentHistories { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<Priorities> Priorities { get; set; }
        public virtual DbSet<RequestTypes> RequestTypes { get; set; }
        public virtual DbSet<RequestTypeSteps> RequestTypeSteps { get; set; }
        public virtual DbSet<ServiceRequestNotes> ServiceRequestNotes { get; set; }
        public virtual DbSet<ServiceRequests> ServiceRequests { get; set; }
        public virtual DbSet<StatusSets> StatusSets { get; set; }
        public virtual DbSet<StatusTypes> StatusTypes { get; set; }
        public virtual DbSet<StepHistories> StepHistories { get; set; }
        public virtual DbSet<TeamAssignmentHistories> TeamAssignmentHistories { get; set; }
        public virtual DbSet<TeamMembers> TeamMembers { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }

        public ServeMeHRCoreContext(DbContextOptions<ServeMeHRCoreContext> options)
    : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adinformations>(entity =>
            {
                entity.ToTable("ADInformations");

                entity.Property(e => e.Appusername)
                    .IsRequired()
                    .HasColumnName("APPUSERNAME")
                    .HasMaxLength(255);

                entity.Property(e => e.Cn)
                    .HasColumnName("cn")
                    .HasMaxLength(255);

                entity.Property(e => e.Company)
                    .HasColumnName("company")
                    .HasMaxLength(255);

                entity.Property(e => e.DisplayName)
                    .HasColumnName("displayName")
                    .HasMaxLength(255);

                entity.Property(e => e.GivenName)
                    .IsRequired()
                    .HasColumnName("givenName")
                    .HasMaxLength(255);

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasMaxLength(255);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(255);

                entity.Property(e => e.SAmaccountName)
                    .IsRequired()
                    .HasColumnName("sAMAccountNAme")
                    .HasMaxLength(255);

                entity.Property(e => e.Sn)
                    .IsRequired()
                    .HasColumnName("sn")
                    .HasMaxLength(255);

                entity.Property(e => e.TelephoneNumber)
                    .HasColumnName("telephoneNumber")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.WwWhomePage)
                    .HasColumnName("wwWHomePage")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ApplicConfs>(entity =>
            {
                entity.Property(e => e.Adactive).HasColumnName("ADActive");

                entity.Property(e => e.AppAdmin).HasMaxLength(255);

                entity.Property(e => e.BackAdmin).HasMaxLength(255);

                entity.Property(e => e.EnableSsl).HasColumnName("EnableSSL");

                entity.Property(e => e.Ldapconn)
                    .HasColumnName("LDAPConn")
                    .HasMaxLength(255);

                entity.Property(e => e.Ldappath)
                    .HasColumnName("LDAPPath")
                    .HasMaxLength(255);

                entity.Property(e => e.ManageHremail)
                    .HasColumnName("ManageHREmail")
                    .HasMaxLength(255);

                entity.Property(e => e.ManageHremailPass)
                    .HasColumnName("ManageHREmailPass")
                    .HasMaxLength(255);

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);

                entity.Property(e => e.Smtphost)
                    .HasColumnName("SMTPHost")
                    .HasMaxLength(255);

                entity.Property(e => e.Smtpport).HasColumnName("SMTPPort");
            });

            modelBuilder.Entity<Appointments>(entity =>
            {
                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.MsgId)
                    .HasColumnName("MsgID")
                    .HasMaxLength(255);

                entity.Property(e => e.Notes).HasMaxLength(255);

                entity.Property(e => e.RecipientEmail)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SenderEmail)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<FileDetails>(entity =>
            {
                entity.HasIndex(e => e.ServiceRequestId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Extension).HasMaxLength(10);

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.ServiceRequestId).HasColumnName("ServiceRequestID");

                entity.HasOne(d => d.ServiceRequest)
                    .WithMany(p => p.FileDetails)
                    .HasForeignKey(d => d.ServiceRequestId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_FileDetail_ServiceRequests");
            });

            modelBuilder.Entity<IndividualAssignmentHistories>(entity =>
            {
                entity.HasIndex(e => e.AssignedTo);

                entity.HasIndex(e => e.ServiceRequest);

                entity.Property(e => e.AssignedBy)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DateAssigned).HasColumnType("datetime");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.IndividualAssignmentHistories)
                    .HasForeignKey(d => d.AssignedTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IndividualAssignmentHistories_Members");

                entity.HasOne(d => d.ServiceRequestNavigation)
                    .WithMany(p => p.IndividualAssignmentHistories)
                    .HasForeignKey(d => d.ServiceRequest)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_dbo_IndividualAssignmentHistories_dbo_ServiceRequests_ServiceRequest");
            });

            modelBuilder.Entity<Members>(entity =>
            {
                entity.Property(e => e.MemberEmail)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberFirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberFullName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberLastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberPhone)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberUserid)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Priorities>(entity =>
            {
                entity.HasIndex(e => e.Team);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.PriorityDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.Priorities)
                    .HasForeignKey(d => d.Team)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_Priorities_dbo_Teams_Team");
            });

            modelBuilder.Entity<RequestTypes>(entity =>
            {
                entity.HasIndex(e => e.Team);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.RequestTypeDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.RequestTypes)
                    .HasForeignKey(d => d.Team)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_RequestTypes_dbo_Teams_Team");
            });

            modelBuilder.Entity<RequestTypeSteps>(entity =>
            {
                entity.HasIndex(e => e.RequestType);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.StepDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.RequestTypeNavigation)
                    .WithMany(p => p.RequestTypeSteps)
                    .HasForeignKey(d => d.RequestType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_RequestTypeSteps_dbo_RequestTypes_RequestType");
            });

            modelBuilder.Entity<ServiceRequestNotes>(entity =>
            {
                entity.HasIndex(e => e.ServiceRequest);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.NoteDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.WrittenBy).HasMaxLength(255);

                entity.HasOne(d => d.ServiceRequestNavigation)
                    .WithMany(p => p.ServiceRequestNotes)
                    .HasForeignKey(d => d.ServiceRequest)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ServiceRequestNotes_ServiceRequests");
            });

            modelBuilder.Entity<ServiceRequests>(entity =>
            {
                entity.HasIndex(e => e.Member);

                entity.HasIndex(e => e.Priority);

                entity.HasIndex(e => e.RequestType);

                entity.HasIndex(e => e.RequestTypeStep);

                entity.HasIndex(e => e.Status);

                entity.HasIndex(e => e.Team);

                entity.Property(e => e.DateTimeCompleted).HasColumnType("datetime");

                entity.Property(e => e.DateTimeStarted).HasColumnType("datetime");

                entity.Property(e => e.DateTimeSubmitted).HasColumnType("datetime");

                entity.Property(e => e.RequestDescription).IsRequired();

                entity.Property(e => e.RequestHeading)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RequestorEmail)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RequestorFirstName).HasMaxLength(255);

                entity.Property(e => e.RequestorId)
                    .IsRequired()
                    .HasColumnName("RequestorID")
                    .HasMaxLength(255);

                entity.Property(e => e.RequestorLastName).HasMaxLength(255);

                entity.Property(e => e.RequestorPhone).HasMaxLength(255);

                entity.HasOne(d => d.MemberNavigation)
                    .WithMany(p => p.ServiceRequests)
                    .HasForeignKey(d => d.Member)
                    .HasConstraintName("FK_dbo_ServiceRequests_dbo_Members_Member");

                entity.HasOne(d => d.PriorityNavigation)
                    .WithMany(p => p.ServiceRequests)
                    .HasForeignKey(d => d.Priority)
                    .HasConstraintName("FK_dbo_ServiceRequests_dbo_Priorities_Priority");

                entity.HasOne(d => d.RequestTypeNavigation)
                    .WithMany(p => p.ServiceRequests)
                    .HasForeignKey(d => d.RequestType)
                    .HasConstraintName("FK_dbo_ServiceRequests_dbo_RequestTypes_RequestType");

                entity.HasOne(d => d.RequestTypeStepNavigation)
                    .WithMany(p => p.ServiceRequests)
                    .HasForeignKey(d => d.RequestTypeStep)
                    .HasConstraintName("FK_dbo_ServiceRequests_dbo_RequestTypeSteps_RequestTypeStep");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.ServiceRequests)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_ServiceRequests_dbo_StatusSets_Status");

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.ServiceRequests)
                    .HasForeignKey(d => d.Team)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_ServiceRequests_dbo_Teams_Team");
            });

            modelBuilder.Entity<StatusSets>(entity =>
            {
                entity.HasIndex(e => e.StatusType);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.StatusTypeNavigation)
                    .WithMany(p => p.StatusSets)
                    .HasForeignKey(d => d.StatusType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_StatusSets_dbo_StatusTypes_StatusType");
            });

            modelBuilder.Entity<StatusTypes>(entity =>
            {
                entity.Property(e => e.StatusTypeDescription)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<StepHistories>(entity =>
            {
                entity.HasIndex(e => e.RequestTypeStep);

                entity.HasIndex(e => e.ServiceRequest);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.HasOne(d => d.RequestTypeStepNavigation)
                    .WithMany(p => p.StepHistories)
                    .HasForeignKey(d => d.RequestTypeStep)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_StepHistories_dbo_RequestTypeSteps_RequestTypeStep");

                entity.HasOne(d => d.ServiceRequestNavigation)
                    .WithMany(p => p.StepHistories)
                    .HasForeignKey(d => d.ServiceRequest)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_dbo_StepHistories_dbo_ServiceRequests_ServiceRequest");
            });

            modelBuilder.Entity<TeamAssignmentHistories>(entity =>
            {
                entity.HasIndex(e => e.ServiceRequest);

                entity.HasIndex(e => e.Team);

                entity.Property(e => e.AssignedBy)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DateAssigned).HasColumnType("datetime");

                entity.HasOne(d => d.ServiceRequestNavigation)
                    .WithMany(p => p.TeamAssignmentHistories)
                    .HasForeignKey(d => d.ServiceRequest)
                    .HasConstraintName("FK_dbo_TeamAssignmentHistories_dbo_ServiceRequests_ServiceRequest");

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.TeamAssignmentHistories)
                    .HasForeignKey(d => d.Team)
                    .HasConstraintName("FK_dbo_TeamAssignmentHistories_dbo_Teams_Team");
            });

            modelBuilder.Entity<TeamMembers>(entity =>
            {
                entity.HasIndex(e => e.Member);

                entity.HasIndex(e => e.Team);

                entity.HasOne(d => d.MemberNavigation)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.Member)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_TeamMembers_dbo_Members_Member");

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.Team)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_TeamMembers_dbo_Teams_Team");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.Property(e => e.TeamDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.TeamEmailAddress)
                    .IsRequired()
                    .HasMaxLength(255);
            });
        }
    }
}
