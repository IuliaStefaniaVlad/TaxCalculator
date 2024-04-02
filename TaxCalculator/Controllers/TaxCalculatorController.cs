using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Api.Controllers
{
    [Route("TaxCalculator")]
    [ApiController]
    public class TaxCalculatorController : ControllerBase
    {
        private readonly ITaxCalculatorService taxCalculatorService;

        public TaxCalculatorController(ITaxCalculatorService taxCalculatorService)
        {
            this.taxCalculatorService = taxCalculatorService ?? throw new ArgumentNullException(nameof(taxCalculatorService));
        }

        [HttpGet]
        [Route("Calculate")]
        public IActionResult Calculate([FromQuery] decimal salary, [FromQuery] string country)
        {
            if (country is not null)
            {
                var taxCalculationResult = taxCalculatorService.Calculate(salary, country);
                if (taxCalculationResult.IsFailed)
                {
                    return BadRequest(taxCalculationResult.Errors);
                }

                return Ok(taxCalculationResult.Value);
            }
            return BadRequest("Input is not valid.");
        }


    }
}
