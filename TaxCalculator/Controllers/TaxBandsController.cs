using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using TaxCalculator.Models.Shared;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Api.Controllers
{
    [Route("TaxBands")]
    [ApiController]
    public class TaxBandsController : ControllerBase
    {
        private readonly ITaxBandsService _taxBandService;
        private readonly ILogger<TaxBandsController> _logger;
        public TaxBandsController(ITaxBandsService taxBandService, ILogger<TaxBandsController> logger)
        {
            _taxBandService = taxBandService ?? throw new ArgumentNullException(nameof(taxBandService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("GetAllTaxBands")]
        public IActionResult GetAllTaxBands()
        {
            _logger.LogInformation($"{Activity.Current?.Id} Get all bands called");

            var taxBand = _taxBandService.GetAll();
            if (taxBand.IsFailed)
            {
                return NotFound(taxBand.Errors);
            }
            return Ok(taxBand.Value);
        }

        [HttpGet]
        [Authorize]
        [Route("GetTaxBandsByCountry")]
        public IActionResult GetTaxBand([Required][FromQuery] string country)
        {
            var taxBand = _taxBandService.GetByCountry(country);
            if (taxBand.IsFailed)
            {
                return NotFound(taxBand.Errors);
            }
            return Ok(taxBand.Value);
        }

        [HttpPost]
        [Authorize]
        [Route("CreateTaxBands")]
        public async Task<IActionResult> CreateCountryTaxBands([FromBody] CountryTaxBandsModel countryTaxBandsModel)
        {
            _logger.LogInformation($"{Activity.Current?.Id} Create called with parameters countryTaxBandsModel:{countryTaxBandsModel.ToString()}");

            var taxBand = await _taxBandService.Add(countryTaxBandsModel);
            if(taxBand.IsFailed)
            {
                return BadRequest(taxBand.Errors);
            }
            return Ok(taxBand.Value);   
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateTaxBands")]
        public async Task<IActionResult> UpdateCountryTaxBands([FromBody] CountryTaxBandsModel countryTaxBandsModel)
        {
            _logger.LogInformation($"{Activity.Current?.Id} Update called with parameters countryTaxBandsModel:{countryTaxBandsModel.ToString()}");

            var taxBand = await _taxBandService.Update(countryTaxBandsModel);
            if (taxBand.IsFailed)
            {
                return BadRequest(taxBand.Errors);
            }
            return Ok(taxBand.Value);
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteTaxBands")]
        public async Task<IActionResult> DeleteCountryTaxBands([FromQuery] int id)
        {
            _logger.LogInformation($"{Activity.Current?.Id} Delete called with parameters id:{id}");
            var taxBand = await _taxBandService.Remove(id);
            if (!taxBand)
            {
                return NotFound(); // If we fail to remove it... most likely it doesn't exist
            }
            return Ok();
        }
    }
}
