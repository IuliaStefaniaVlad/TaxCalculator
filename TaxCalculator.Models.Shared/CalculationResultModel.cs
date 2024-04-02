namespace TaxCalculator.Models.Shared;

public record CalculationResultModel
{
    public required decimal GrossAnnualSalary { get; set; }
    public required decimal GrossMonthlySalary { get; set;}
    public required decimal NetAnnualSalary { get; set; }
    public required decimal NetMonthlySalary { get; set; }
    public required decimal AnnualTax { get; set; }
    public required decimal MonthlyTax { get; set; }

}
