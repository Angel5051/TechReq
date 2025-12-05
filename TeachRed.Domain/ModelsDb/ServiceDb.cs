using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechReq.Domain.ModelsDb
{
    [Table("Services")]
    public class ServiceDb
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Required]
        [Column("Name")]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        [Column("Description")]
        [MaxLength(1000)]
        public required string Description { get; set; }

        // --- НОВЫЕ ПОЛЯ ---
        [Column("Price")]
        public decimal Price { get; set; } // Цена

        [Column("Category")]
        public string Category { get; set; } = "all"; // Категория (maintenance, tires и т.д.)

        [Column("IsExpress")]
        public bool IsExpress { get; set; } // Экспресс услуга или нет

        // Связь с файлами (оставляем как было)
        public ICollection<RequestFileDb>? RequestFiles { get; set; }
    }
}