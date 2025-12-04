using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachRed.Domain.Enum;
using TechReq.Domain.ModelsDb;

namespace TechReq.Domain.ModelsDb
{
    [Table("Users")]
    public class UsersDb
    {
        //
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Required]
        [Column("Login")]
        [MaxLength(100)]
        public required string Login { get; set; }

        [Required]
        [Column("Password")]
        [MaxLength(255)]
        public required string Password { get; set; }

        [Required]
        [Column("Email")]
        [MaxLength(255)]
        public required string Email { get; set; }

        [Column("Role")]
        public Role Role { get; set; }

        [Column("CreatedAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        //

        // Navigation properties
        public virtual ICollection<RequestDb> Requests { get; set; }
    }
}