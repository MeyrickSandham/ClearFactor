using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClearFactorModelEntity.Models
{
    [Table(nameof(Car), Schema = "dbo")]
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Make { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Model { get; set; }

        [Range(1900, 2500)]
        public int Year { get; set; }

        [StringLength(15, MinimumLength = 1)]
        public string Color { get; set; }

        [StringLength(15, MinimumLength = 1)]
        public string RegistrationId { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public string VinNumber { get; set; }

        public Guid DriverId { get; set; }
    }
}
