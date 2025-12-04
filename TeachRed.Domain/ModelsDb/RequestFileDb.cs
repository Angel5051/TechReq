using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechReq.Domain.ModelsDb
{
    [Table("RequestFiles")]
    public class RequestFileDb
    {
        //
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Required]
        [Column("FileName")]
        [MaxLength(500)]
        public required string FileName { get; set; }

        [Required]
        [Column("FilePath")]
        [MaxLength(1000)]
        public required string FilePath { get; set; }

        [Column("UploadedAt", TypeName = "timestamp")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;


        // --- Внешний ключ ---
        [Required]
        [Column("Request_id")]
        public int RequestId { get; set; }
        //
    }
}