using System.ComponentModel.DataAnnotations;

namespace Ruleta.Application.Dtos
{
    public class UpdateFundsRequest
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Los fondos deben ser mayores a 0")]
        public decimal Funds { get; set; }
        public ETransaction Transaction { get; set; }
    }

    public enum ETransaction
    {
        Add,
        Withdraw
    }
}
