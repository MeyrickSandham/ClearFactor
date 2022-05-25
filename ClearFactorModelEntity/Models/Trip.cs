using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClearFactorModelEntity.Models
{
    [Table(nameof(Trip), Schema = "dbo")]
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string StartLocation { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string EndLocation { get; set; }

        public bool Active { get; set; }

        public int HeartBeatCount { get; set; }
        
        public Guid CarId { get; set; }        
    }
}
