using Microsoft.AspNetCore.Components;
using TaxCalculator.Client.Interfaces;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Client.Components.Pages
{
    public partial class Calculator
    {
        [Inject]
        ITaxBandsClient taxBandsClient { get; set; }
        [Inject]
        ITaxCalculatorClient taxCalculatorClient { get; set; }
        CountryTaxBandsModel? selectedCountry;
        List<CountryTaxBandsModel> countryTaxBandsModels = [];
        decimal grossAnnualSalary;
        CalculationResultModel? calculationResult;
        bool hidden = true;
        string error = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var result = await taxBandsClient.GetAllTaxBands();
            if (result.IsSuccess)
            {
                countryTaxBandsModels = result.Value.ToList();
            }
        }
        public async void CalculateTax()
        {
            hidden = true;
            error = string.Empty; 
            if(selectedCountry is null)
            {
                hidden = false;
                error = "Please select a country";
                return; 
            }
            var result =  await taxCalculatorClient.Calculate(grossAnnualSalary, selectedCountry.Country);
            if(result.IsSuccess)
            {
                calculationResult = result.Value;
                StateHasChanged();
            }

        }

    }
}