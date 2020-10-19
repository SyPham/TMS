using Data.Extensions;
using Data.Models;
using Data.ViewModel.Task;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Data
{
    public class DataContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Data.Models.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<OC> OCs { get; set; }
        public DbSet<OCUser> OCUsers { get; set; }
        public DbSet<FromWho> FromWhos { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Deputy> Deputies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationDetail> NotificationDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentDetail> CommentDetails { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<UserJoinHub> UserJoinHubs { get; set; }
        public DbSet<UploadImage> UploadImages { get; set; }
        public DbSet<CheckTask> CheckTasks { get; set; }
        public DbSet<SystemCode> SystemCodes { get; set; }
        public DbSet<UserSystem> UserSystems { get; set; }
        public DbSet<SignInHistory> SignInHistories { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OCUser>().HasKey(ba => new { ba.UserID, ba.OCID });

            builder.Entity<User>()
             .HasMany(u => u.Tags)
             .WithOne(c => c.User)
             .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                      .HasMany(u => u.NotificationDetails)
                      .WithOne(c => c.User)
                      .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                    .HasMany(u => u.Projects)
                    .WithOne(c => c.User)
                    .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                                .HasMany(u => u.UserSystems)
                                .WithOne(c => c.User)
                                .OnDelete(DeleteBehavior.ClientCascade);

          
            builder.Entity<User>()
                .HasMany(u => u.Follows)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
           .HasMany(u => u.Deputies)
           .WithOne(c => c.User)
           .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                .HasMany(u => u.TeamMembers)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                          .HasMany(u => u.Managers)
                          .WithOne(c => c.User)
                          .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Project>()
                          .HasMany(u => u.Tasks)
                          .WithOne(c => c.Project)
                          .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<OC>()
                   .HasMany(u => u.OCUsers)
                   .WithOne(c => c.OC)
                   .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<OC>()
                  .HasMany(u => u.Tasks)
                  .WithOne(c => c.OC)
                  .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                   .HasMany(u => u.OCUsers)
                   .WithOne(c => c.User)
                   .OnDelete(DeleteBehavior.ClientCascade);


            builder.Entity<Data.Models.Task>()
                .HasMany(u => u.Tags)
                .WithOne(c => c.Task)
                .OnDelete(DeleteBehavior.ClientCascade);



            builder.Entity<Data.Models.Task>()
                .HasOne(u => u.Tutorial)
                .WithOne(c => c.Task)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Data.Models.Task>()
                 .HasMany(u => u.Deputies)
                 .WithOne(c => c.Task)
                 .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Data.Models.Task>()
                 .HasMany(u => u.Follows)
                 .WithOne(c => c.Task)
                 .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Data.Models.Task>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.Task)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Data.Models.Comment>()
                 .HasMany(u => u.CommentDetails)
                 .WithOne(c => c.Comment)
                 .OnDelete(DeleteBehavior.ClientCascade);


            builder.Entity<Project>()
              .HasMany(u => u.Tutorials)
              .WithOne(c => c.Project)
              .OnDelete(DeleteBehavior.ClientCascade);


            builder.Entity<Project>()
              .HasMany(u => u.Managers)
              .WithOne(c => c.Project)
              .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Project>()
            .HasMany(u => u.TeamMembers)
            .WithOne(c => c.Project)
            .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Project>()
           .HasMany(u => u.Tasks)
           .WithOne(c => c.Project)
           .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Project>()
           .HasOne(u => u.RoomTable)
           .WithOne(c => c.Project)
           .OnDelete(DeleteBehavior.ClientCascade);


        }
    }
}
