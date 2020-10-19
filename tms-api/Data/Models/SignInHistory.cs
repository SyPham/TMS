using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
   public class SignInHistory
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Token { get; set; }
        public string Username  { get; set; }
        public string Host { get; set; }
        public int UserID  { get; set; }
        public int SystemCode { get; set; }
        [ForeignKey("SystemCode")]
        public SystemCode SystemCodeTbl { get; set; }

        public DateTime LoginTime { get; set; }
        public DateTime? LastRequestEndTime { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
