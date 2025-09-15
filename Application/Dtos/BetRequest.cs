using System.ComponentModel.DataAnnotations;

namespace Ruleta.Application.Dtos
{
    public class BetRequest
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "La apuesta debe ser mayor a 0")]
        public decimal Bet { get; set; }
        [Range(0, 36, ErrorMessage = "El número elegido debe estar entre 0 y 36")]
        public int? ChosenNumber { get; set; }
        public EColor ChosenColor { get; set; }
        public ECondition? ChosenCondition { get; set; }
    }

    public enum EColor
    {
        Red,
        Black
    }

    public enum ECondition
    {
        Even,
        Odd
    }
}
