using Ruleta.Application.Dtos;
using Ruleta.Application.Interfaces;

namespace Ruleta.Application.Services
{
    public class BetService : IBetService
    {
        public BetResponse ResolveBet(BetRequest request)
        {
            if (request.ChosenNumber.HasValue && request.ChosenCondition.HasValue)
            {
                throw new ArgumentException("Número elegido y condición elegida no pueden seleccionados al mismo tiempo");
            }

            BetResponse response = new();
            Random random = new Random();
            Array colors = Enum.GetValues(typeof(EColor));
            Array conditions = Enum.GetValues(typeof(ECondition));

            int generatedNumber = random.Next(37);
            var generatedColor = colors.GetValue(random.Next(colors.Length));
            var generatedCondition = conditions.GetValue(random.Next(conditions.Length));

            response.GeneratedNumber = generatedNumber;
            response.GeneratedColor = (EColor)generatedColor!;
            response.GeneratedCondition = (ECondition)generatedCondition!;

            if (request.ChosenNumber == generatedNumber && request.ChosenColor.Equals(generatedColor))
            {
                response.Gains = request.Bet * 3;
                response.GainsType = "Apuesta triplicada";
            }
            else if (request.ChosenCondition.Equals(generatedCondition) && request.ChosenColor.Equals(generatedColor))
            {
                response.Gains = request.Bet;
                response.GainsType = "Apuesta igualada";
            }
            else if (request.ChosenColor.Equals(generatedColor))
            {
                response.Gains = Math.Round(request.Bet / 3, 2);
                response.GainsType = "Tercio de puesta";
            }

            return response;
        }
    }
}
