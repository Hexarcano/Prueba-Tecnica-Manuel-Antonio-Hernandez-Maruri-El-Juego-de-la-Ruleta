namespace Ruleta.Application.Dtos
{
    public class BetResponse
    {
        public decimal Gains { get; set; } = decimal.Zero;
        public int GeneratedNumber { get; set; }
        public string GeneratedColor { get; set; }
        public string GeneratedCondition { get; set; }
        public string GainsType { get; set; } = "No ganaste";
    }
}
