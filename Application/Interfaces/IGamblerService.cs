using Ruleta.Application.Dtos;

namespace Ruleta.Application.Interfaces
{
    public interface IGamblerService
    {
        Task<GamblerResponse> RegisterGambler(RegisterGamblerRequest request);
        Task<GamblerResponse> AddFunds(string name, UpdateFundsRequest request);
        Task<GamblerResponse> WithdrawFunds(string name, UpdateFundsRequest request);
        Task<GamblerResponse> GetGambler(string name);
    }
}
