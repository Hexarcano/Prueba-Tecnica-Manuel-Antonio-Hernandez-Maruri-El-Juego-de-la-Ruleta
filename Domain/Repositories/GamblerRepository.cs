using Microsoft.EntityFrameworkCore;
using Ruleta.Domain.Dto;
using Ruleta.Domain.Entities;
using Ruleta.Domain.Interface;
using Ruleta.Infraestructure.Context;
using System;

namespace Ruleta.Domain.Repository
{
    public class GamblerRepository : IGamblerRepository
    {
        private readonly RuletaContext _context;

        public GamblerRepository(RuletaContext context)
        {
            _context = context;
        }

        public async Task<Gambler?> GetGamblerByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return await _context.Gamblers.FirstOrDefaultAsync(g => g.Name == name);
        }

        public Task<IEnumerable<Gambler>> GetGamblersAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<Gambler> CreateGamblerAsync(Gambler gambler)
        {
            _context.Gamblers.Add(gambler);

            await _context.SaveChangesAsync();

            return gambler;
        }

        public async Task<Gambler> UpdateGamblerAsync(Gambler gambler)
        {
            _context.Gamblers.Update(gambler);

            await _context.SaveChangesAsync();

            return gambler;
        }
        public Task<bool> DeleteByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
