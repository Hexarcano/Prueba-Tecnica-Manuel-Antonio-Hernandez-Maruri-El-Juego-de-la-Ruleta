using System.ComponentModel.DataAnnotations;

namespace Ruleta.Application.Dtos
{
    public class GamblerResponse
    {
        public string Name { get; set; } = string.Empty;
        public decimal? Funds { get; set; }
    }
}
