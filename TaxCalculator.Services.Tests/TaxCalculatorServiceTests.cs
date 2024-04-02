using FluentResults;
using Moq;
using TaxCalculator.Models.Shared;
using TaxCalculator.Repositories.Interfaces;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Services.Tests
{
    public class TaxCalculatorServiceTests
    {
        private  Mock<ICountryTaxBandsRepository> countryTaxBandsRepository = new Mock<ICountryTaxBandsRepository>();
        private  Mock<ITaxCalculator> taxCalculator = new Mock<ITaxCalculator>();

        [Fact]
        public void Calculate_OK()
        {
            decimal salary = 10000;
            string countryName = "England";
            var countryTaxBands = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
                Country = countryName,
                TaxBands = new List<TaxBandModel>()
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
                                }
            };
            var expectedCalculationResult = new CalculationResultModel() 
            {
                GrossAnnualSalary = 10000, 
                GrossMonthlySalary = (decimal)833.33, 
                NetAnnualSalary = 9000, 
                NetMonthlySalary = 750, 
                AnnualTax = 1000, 
                MonthlyTax = (decimal)83.33
            };
            countryTaxBandsRepository.Setup(x => x.GetByCountry(countryName)).Returns(countryTaxBands);
            taxCalculator.Setup(x => x.Calculate(salary, countryTaxBands.TaxBands)).Returns(expectedCalculationResult);
            var service = new TaxCalculatorService(countryTaxBandsRepository.Object, taxCalculator.Object);

            //act
            var result = service.Calculate(salary, countryName);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedCalculationResult, result.Value);
        }

        [Fact]
        public void Calculate_CountryNotFound()
        {
            //arrange
            decimal salary = 10000;
            string countryName = "Romania";
            countryTaxBandsRepository.Setup(x => x.GetByCountry(countryName)).Returns(Result.Fail("Couldn't find country"));
            taxCalculator.Setup(x => x.Calculate(salary, It.IsAny<ICollection<TaxBandModel>>())).Returns(It.IsAny<Result<CalculationResultModel>>());
            var service = new TaxCalculatorService(countryTaxBandsRepository.Object, taxCalculator.Object);

            //act
            var result = service.Calculate(salary, countryName);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Calculate_TaxBandsNull()
        {
            decimal salary = 10000;
            string countryName = "Romania";
            var countryTaxBands = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
                Country = countryName,
                TaxBands = null!
            };
            countryTaxBandsRepository.Setup(x => x.GetByCountry(countryName)).Returns(countryTaxBands);
            taxCalculator.Setup(x => x.Calculate(salary, It.IsAny<ICollection<TaxBandModel>>())).Throws(new ArgumentNullException("tax bands"));
            var service = new TaxCalculatorService(countryTaxBandsRepository.Object, taxCalculator.Object);

            //assert & act
            Assert.Throws<ArgumentNullException>(() => service.Calculate(salary, countryName));
        }
    }
}
