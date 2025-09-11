using System.ComponentModel.DataAnnotations;

namespace Ruleta.Application.Dtos
{
    public class RegisterGamblerRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters long")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Funds must be greater than or equal to 0")]
        public decimal? Funds { get; set; }
    }
}
