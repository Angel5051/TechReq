using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using TeachRed.Domain.Enum;

namespace TechReq.Domain.ModelsDb
{
    [Table("Requests")]
    public class RequestDb
    {
        //
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Required]
        [Column("Title")]
        [MaxLength(255)]
        public required string Title { get; set; }

        [Required]
        [Column("Description")]
        [MaxLength(4000)]
        public required string Description { get; set; }

        [Column("Status")]
        public StatusRequest Status { get; set; } // Например, 1: Создана, 2: В работе, 3: Закрыта

        [Column("Assigned_staff_id")] // Назначенный сотрудник (UsersDb)
        public Guid? AssignedStaffId { get; set; } // Может быть null

        [Column("CreatedAt", TypeName = "timestamp")]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // --- Внешние ключи ---

        [Required]
        [Column("User_id")] // Создатель заявки (UsersDb)
        public Guid UserId { get; set; }

        [Required]
        [Column("Services_id")] // Связанный сервис (ServiceDb)q1
        public Guid ServiceId { get; set; }
        //
        public ICollection<RequestFileDb> Files { get; set; }


    }
}