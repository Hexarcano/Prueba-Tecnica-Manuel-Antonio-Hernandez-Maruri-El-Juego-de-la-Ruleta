namespace Ruleta.Application.Dtos
{
    public class BetResponse
    {
        public decimal Gains { get; set; } = decimal.Zero;
        public int GeneratedNumber { get; set; }
        public EColor GeneratedColor { get; set; }
        public ECondition GeneratedCondition { get; set; }
        public string GainsType { get; set; } = "No ganaste";
    }
}
