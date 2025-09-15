using Ruleta.Application.Dtos;
using Ruleta.Application.Interfaces;
using Ruleta.Domain.Entities;
using Ruleta.Domain.Interfaces;

namespace Ruleta.Application.Services
{
    public class GamblerService : IGamblerService
    {
        private readonly IGamblerRepository _repository;

        public GamblerService(IGamblerRepository repository)
        {
            _repository = repository;
        }

        public async Task<GamblerResponse> GetGambler(string name)
        {
            Gambler? gambler = await _repository.GetGamblerByNameAsync(name);

            if (gambler == null)
            {
                throw new KeyNotFoundException("Jugador no encontrado");
            }

            return new GamblerResponse
            {
                Name = gambler.Name,
                Funds = gambler.Funds
            };
        }

        public async Task<GamblerResponse> RegisterGambler(RegisterGamblerRequest request)
        {
            Gambler? gambler = await _repository.GetGamblerByNameAsync(request.Name);

            if (gambler != null)
            {
                if (request.Funds == null)
                {
                    throw new ArgumentNullException(nameof(request.Funds), "Los fondos deben ser definidos.");
                }

                UpdateFundsRequest updateFundsRequest = new UpdateFundsRequest
                {
                    Funds = request.Funds.Value,
                };

                return await AddFunds(request.Name, updateFundsRequest);
            }

            gambler = new Gambler
            {
                Name = request.Name,
                Funds = request.Funds,
            };

            Gambler saved = await _repository.CreateGamblerAsync(gambler);

            return new GamblerResponse
            {
                Name = saved.Name,
                Funds = saved.Funds
            };
        }

        public async Task<GamblerResponse> AddFunds(string name, UpdateFundsRequest request)
        {
            Gambler? gambler = await _repository.GetGamblerByNameAsync(name);

            if (gambler == null)
            {
                throw new KeyNotFoundException("Jugador no encontrado");
            }

            if (request.Funds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Funds), "Los fondos deben ser positivos");
            }

            gambler.Funds += request.Funds;

            await _repository.UpdateGamblerAsync(gambler);

            return new GamblerResponse
            {
                Name = gambler.Name,
                Funds = gambler.Funds
            };
        }

        public async Task<GamblerResponse> WithdrawFunds(string name, UpdateFundsRequest request)
        {
            Gambler? gambler = await _repository.GetGamblerByNameAsync(name);

            if (gambler == null)
            {
                throw new KeyNotFoundException("Jugador no encontrado");
            }

            if (request.Funds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Funds), "Los fondos deben ser positivos");
            }

            if (request.Funds > gambler.Funds)
            {
                throw new InvalidOperationException($"Fondos insuficientes. Tienes: {gambler.Funds}, y solicitaste retirar: {request.Funds}.");
            }

            gambler.Funds -= request.Funds;

            await _repository.UpdateGamblerAsync(gambler);

            return new GamblerResponse
            {
                Name = gambler.Name,
                Funds = gambler.Funds
            };
        }
    }
}
