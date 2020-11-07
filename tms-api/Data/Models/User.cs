using Data.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    [JsonObject(IsReference = true)]
    public class User : IEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public int OCID { get; set; }
        public int LevelOC { get; set; }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public string ImageURL { get; set; }
        public string AccessTokenLineNotify { get; set; }
        public byte[] ImageBase64 { get; set; }
        public bool isLeader { get; set; }
        public bool IsShow { get; set; }
        public int DeleteBy { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string VerificationToken { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime ModifyTime { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<NotificationDetail> NotificationDetails { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<UserSystem> UserSystems { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<OCUser> OCUsers { get; set; }
        public virtual ICollection<Deputy> Deputies { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
