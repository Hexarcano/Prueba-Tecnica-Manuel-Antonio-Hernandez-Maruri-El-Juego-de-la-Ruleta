using Ruleta.Domain.Entities;

namespace Ruleta.Domain.Interfaces
{
    public interface IGamblerRepository
    {
        Task<IEnumerable<Gambler>> GetGamblersAsync();
        Task<Gambler?> GetGamblerByNameAsync(string name);
        Task<Gambler> CreateGamblerAsync(Gambler gambler);
        Task<Gambler> UpdateGamblerAsync(Gambler gambler);
        Task<bool> DeleteByNameAsync(string name);
    }
}
