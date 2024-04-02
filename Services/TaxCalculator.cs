using FluentResults;
using TaxCalculator.Models.Shared;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Services
{

    public class TaxCalculator : ITaxCalculator
    {
        public Result<CalculationResultModel> Calculate(decimal grossAnnualSalary, ICollection<TaxBandModel> taxBands)
        {
            var annualPaidTax = CalculateAnnualTax(grossAnnualSalary, taxBands);
            return new CalculationResultModel()
            {
                GrossAnnualSalary = grossAnnualSalary,
                GrossMonthlySalary = grossAnnualSalary / 12,
                NetAnnualSalary = grossAnnualSalary - annualPaidTax,
                NetMonthlySalary = (grossAnnualSalary - annualPaidTax) / 12,
                AnnualTax = annualPaidTax,
                MonthlyTax = annualPaidTax / 12
            };
        }

        private static decimal CalculateAnnualTax(decimal grossAnnualSalary, ICollection<TaxBandModel> taxBands)
        {
            var validationResult = TaxBandsValidationHelper.ValidateTaxBandInput(taxBands);
           
            if (validationResult.IsFailed)
            {
                throw new ArgumentNullException(string.Join(" \n ", validationResult.Errors.Select(e => e.Message)));
            }

            decimal taxes = 0;
            decimal notTaxedSalary = grossAnnualSalary;

            foreach(var taxBand in taxBands )
            {
                
                if (taxBand.MaxRange is not null)
                {
                    //not last taxBand
                    decimal valueToTax = Math.Min((decimal)(taxBand.MaxRange - taxBand.MinRange), notTaxedSalary);
                    taxes +=  valueToTax * (decimal)taxBand.TaxRate/100; //explicit cast to decimal for TaxRate is required.
                    notTaxedSalary -= valueToTax;
                }
                else 
                {
                    //last tax band
                    taxes += notTaxedSalary * (decimal)taxBand.TaxRate / 100;
                }
            }

            return taxes;

        }

    }
}
