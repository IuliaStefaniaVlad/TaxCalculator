using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.Metrics;
using TaxCalculator.Api.Controllers;
using TaxCalculator.Models.Shared;
using TaxCalculator.Services.Interfaces;
using Xunit;

namespace TaxCalculator.Api.Tests
{
    public class TaxCalculatorControllerTests
    {
        private Mock<ITaxCalculatorService> mockTaxCalculatorService = new Mock<ITaxCalculatorService>();

        [Fact]
        public void Calculate_OK()
        {
            //arrange
            decimal salary = 40000;
            string country = "England";
            var expectedResult = new CalculationResultModel()
            {
                GrossAnnualSalary = salary,
                GrossMonthlySalary = salary / 12,
                NetAnnualSalary = salary - 11000,
                NetMonthlySalary = (salary - 11000) / 12,
                AnnualTax = 11000,
                MonthlyTax = 11000 / 12

            };
            mockTaxCalculatorService.Setup(x => x.Calculate(salary, country)).Returns(expectedResult);
            var controller = new TaxCalculatorController(mockTaxCalculatorService.Object);

            //act
            var result = controller.Calculate(salary, country);

            //assert
            Assert.NotNull(result);
            var okResponse = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(okResponse.StatusCode, 200);
            var actualResult = Assert.IsType<CalculationResultModel>(okResponse.Value);
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async void Calculate_InvalidInput()
        {
            //arrange
            mockTaxCalculatorService.Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<string>())).Returns(new CalculationResultModel() { AnnualTax = 0, GrossAnnualSalary = 0, GrossMonthlySalary=0, MonthlyTax = 0, NetAnnualSalary = 0, NetMonthlySalary = 0 });
            var controller = new TaxCalculatorController(mockTaxCalculatorService.Object);

            //act
            var result = controller.Calculate(0, null);

            //assert
            Assert.NotNull(result);
            var brResponse = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, brResponse.StatusCode);
            Assert.NotNull(brResponse.Value);
        }

        [Fact]
        public async void Calculate_Fail()
        {
            //arrange
            decimal salary = 0; 
            string country = "test";
            mockTaxCalculatorService.Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<string>())).Returns(Result.Fail("Something went wrong."));
            var controller = new TaxCalculatorController(mockTaxCalculatorService.Object);

            //act
            var result = controller.Calculate(salary, country);

            //assert
            Assert.NotNull(result);
            var brResponse = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, brResponse.StatusCode);
            Assert.NotNull(brResponse.Value);
        }
    }
}
