using Ruleta.Application.Dtos;

namespace Ruleta.Application.Interfaces
{
    public interface IBetService
    {
        BetResponse ResolveBet(BetRequest request);
    }
}
