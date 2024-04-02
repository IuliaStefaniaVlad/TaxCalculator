using TaxCalculator.Models.Shared;

namespace TaxCalculator.Services.Tests
{
    public class TaxCalculatorTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1000, 83.33, 1000, 83.33, 0, 0)]
        [InlineData(5000, 416.67, 5000, 416.67, 0, 0)]
        [InlineData(10000, 833.33, 9000, 750, 1000, 83.33)]
        [InlineData(20000, 1666.67, 17000, 1416.67, 3000, 250)]
        [InlineData(40000, 3333.33, 29000, 2416.67, 11000, 916.67)]
        public void Calculate_OK(decimal grossAnnualSalary, 
                                 decimal expectedGrossMonthlySalary, 
                                 decimal expectedNetAnnualSalary, 
                                 decimal expectedNetMonthlySalary, 
                                 decimal expectedAnnualTaxPaid, 
                                 decimal expectedMonthlyTaxPaid)
        {
            //arrange
            var taxBands = new List<TaxBandModel>()
            {
                new TaxBandModel()
                {
                    BandOrder = 1,
                    MinRange = 0,
                    MaxRange = 5000,
                    TaxRate = 0
                },
                new TaxBandModel()
                {
                    BandOrder = 2,
                    MinRange = 5000,
                    MaxRange = 20000,
                    TaxRate = 20
                },
                new TaxBandModel()
                {
                    BandOrder = 3,
                    MinRange = 20000,
                    TaxRate = 40
                }
            };

            var calculator = new TaxCalculator();

            //act
            var result = calculator.Calculate(grossAnnualSalary, taxBands);
            
            //assert

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedGrossMonthlySalary, Math.Round(result.Value.GrossMonthlySalary,2));
            Assert.Equal(expectedNetAnnualSalary, Math.Round(result.Value.NetAnnualSalary,2));
            Assert.Equal(expectedNetMonthlySalary, Math.Round(result.Value.NetMonthlySalary, 2));
            Assert.Equal(expectedAnnualTaxPaid, Math.Round(result.Value.AnnualTax, 2));
            Assert.Equal(expectedMonthlyTaxPaid, Math.Round(result.Value.MonthlyTax, 2));
        }

        [Fact]
        public void Calculate_Fail()
        {
            //arrange
            decimal grossAnnualSalary = 1000;
            var calculator = new TaxCalculator();

            //assert & act
            Assert.Throws<ArgumentNullException>(() => calculator.Calculate(grossAnnualSalary, null));
        }
    }
}
