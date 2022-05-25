using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClearFactorModelEntity.Models
{
    [Table(nameof(Driver), Schema = "dbo")]
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Surname { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Email { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string CellContact { get; set; }
    }
}
