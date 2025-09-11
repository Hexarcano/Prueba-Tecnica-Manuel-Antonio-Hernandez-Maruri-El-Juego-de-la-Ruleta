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
                throw new Exception("Gambler not found");
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
                    throw new Exception("Funds must be defined.");
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
                throw new Exception("Gambler not found");
            }

            if (request.Funds <= 0)
            {
                throw new Exception("Funds must be positive");
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
                throw new Exception("Gambler not found");
            }

            if (request.Funds <= 0)
            {
                throw new Exception("Funds must be positive");
            }

            if (request.Funds > gambler.Funds)
            {
                throw new Exception($"Insufficient Funds. You have: {gambler.Funds}, and you requested to withdraw: {request.Funds}.");
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
