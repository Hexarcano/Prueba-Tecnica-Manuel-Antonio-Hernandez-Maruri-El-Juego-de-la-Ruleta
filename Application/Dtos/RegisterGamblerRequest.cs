using System.ComponentModel.DataAnnotations;

namespace Ruleta.Application.Dtos
{
    public class RegisterGamblerRequest
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Los fondos deben ser mayores o iguales a 0")]
        public decimal? Funds { get; set; }
    }
}
