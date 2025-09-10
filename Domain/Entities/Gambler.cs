using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ruleta.Domain.Entities
{
    public class Gambler
    {
        [Key]
        public required string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Funds must be positive.")]
        public decimal? Funds { get; set; }
    }
}
