using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ruleta.Application.Dtos;
using Ruleta.Application.Interfaces;
using Ruleta.Application.Services;

namespace Ruleta.Infraestructure.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetController(IBetService betService)
        {
            _betService = betService;
        }

        [HttpPost]
        public ActionResult<BetResponse> Get(BetRequest request)
        {
            try
            {
                BetResponse response = _betService.ResolveBet(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
