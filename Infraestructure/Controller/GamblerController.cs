using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Ruleta.Application.Dtos;
using Ruleta.Application.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ruleta.Infraestructure.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamblerController : ControllerBase
    {
        private readonly IGamblerService _gamblerService;

        public GamblerController(IGamblerService gamblerService)
        {
            _gamblerService = gamblerService;
        }

        // GET api/<GamblerController>/name
        [HttpGet("{name}")]
        public async Task<ActionResult<GamblerResponse>> Get(string name)
        {
            try
            {
                GamblerResponse response = await _gamblerService.GetGambler(name);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/<GamblerController>
        [HttpPost]
        public async Task<ActionResult<GamblerResponse>> Post([FromBody] RegisterGamblerRequest request)
        {
            try
            {
                GamblerResponse response = await _gamblerService.RegisterGambler(request);

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<GamblerController>/name
        [HttpPut("{name}")]
        public async Task<ActionResult<GamblerResponse>> PutAsync(string name, [FromBody] UpdateFundsRequest request)
        {
            GamblerResponse response;
            try
            {
                if (request.Transaction == ETransaction.Add)
                {
                    response = await _gamblerService.AddFunds(name, request);
                    return Ok(response);
                }

                if (request.Transaction == ETransaction.Withdraw)
                {
                    response = await _gamblerService.WithdrawFunds(name, request);
                    return Ok(response);
                }

                return BadRequest("Tipo de transacción no válida");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<GamblerController>/name
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
