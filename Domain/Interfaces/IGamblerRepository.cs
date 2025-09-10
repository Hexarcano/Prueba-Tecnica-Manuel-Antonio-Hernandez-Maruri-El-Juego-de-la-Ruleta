using Ruleta.Domain.Dto;
using Ruleta.Domain.Entities;

namespace Ruleta.Domain.Interface
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
