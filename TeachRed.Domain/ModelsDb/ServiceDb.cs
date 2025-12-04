using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechReq.Domain.ModelsDb
{
    [Table("Services")]
    public class ServiceDb
    {
        //РАЗНЫЕ СЕРВИСЫ ТО
        //
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
        //

        ICollection<RequestFileDb> RequestFiles { get; set; }
    }
}