using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaxCalculator.Api.Controllers;
using TaxCalculator.Models.Shared;
using TaxCalculator.Services.Interfaces;
using Xunit;

namespace TaxCalculator.Api.Tests
{
    public class TaxBandsControllerTests
    {
        private Mock<ITaxBandsService> mockTaxBandService = new Mock<ITaxBandsService>();
        private Mock<ILogger<TaxBandsController>> mockLogger = new Mock<ILogger<TaxBandsController>>();

        [Fact]
        public void GetAllTaxBands_OK()
        {
            //arrange
            var taxBands = new List<CountryTaxBandsModel>() {
                new CountryTaxBandsModel()
                {
                    Country = "England",
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
                },
                new CountryTaxBandsModel()
                {
                    Country = "Romania",
                    TaxBands = new List<TaxBandModel>()
                    {
                        new TaxBandModel()
                        {
                            BandOrder = 1,
                            MinRange = 0,
                            MaxRange = 10000,
                            TaxRate = 0
                        },
                        new TaxBandModel()
                        {
                            BandOrder = 2,
                            MinRange = 10000,
                            TaxRate = 40
                        }
                    }
                }
            };
            mockTaxBandService.Setup(x => x.GetAll()).Returns(taxBands);
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = controller.GetAllTaxBands();

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            Assert.Equal(taxBands, okResult.Value);

        }

        [Fact]
        public void GetAllTaxBands_Fail()
        {
            //arrange
            mockTaxBandService.Setup(x => x.GetAll()).Returns(Result.Fail("Could not find data."));
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = controller.GetAllTaxBands();

            //assert
            Assert.NotNull(result);
            var brResponse = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, brResponse.StatusCode);
            Assert.NotNull(brResponse.Value);
        }

        [Fact]
        public void GetTaxBand_OK()
        {
            //arrange
            string countryName = "England";
            var expectedData = new CountryTaxBandsModel()
            {
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
            mockTaxBandService.Setup(x => x.GetByCountry(countryName)).Returns(expectedData);
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = controller.GetTaxBand(countryName);

            //assert
            Assert.NotNull(result);
            var okResponse = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResponse.StatusCode);
            Assert.Equal(expectedData, okResponse.Value);

        }

        [Fact]
        public void GetTaxBand_Fail()
        {
            //arrange
            mockTaxBandService.Setup(x => x.GetByCountry(It.IsAny<string>())).Returns(Result.Fail("Could not find data."));
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = controller.GetTaxBand("Test");

            //assert
            Assert.NotNull(result);
            var brResponse = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, brResponse.StatusCode);
            Assert.NotNull(brResponse.Value);
        }

        [Fact]
        public async Task CreateCountryTaxBands_OK()
        {
            //arrange
            var inputData = new CountryTaxBandsModel()
            {
                Country = "Test",
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
            var expectedData = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
                Country = "Test",
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
            mockTaxBandService.Setup(x => x.Add(inputData)).ReturnsAsync(expectedData);
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = await controller.CreateCountryTaxBands(inputData);

            //assert
            Assert.NotNull(result);
            var okResponse = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResponse.StatusCode);
            Assert.Equal(expectedData, okResponse.Value);
        }

        [Fact]
        public async Task CreateCountryTaxBands_Fail()
        {
            //arrange
            var inputData = new CountryTaxBandsModel()
            {
                Country = "Test",
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
            mockTaxBandService.Setup(x => x.Add(It.IsAny<CountryTaxBandsModel>())).ReturnsAsync(Result.Fail("Something went wrong."));
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = await controller.CreateCountryTaxBands(inputData);

            //assert
            Assert.NotNull(result);
            var brResponse = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, brResponse.StatusCode);
            Assert.NotNull(brResponse.Value);
        }

        [Fact]
        public async Task UpdateCountryTaxBands_OK()
        {
            //arrange
            var data = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
                Country = "Test",
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

            mockTaxBandService.Setup(x => x.Update(data)).ReturnsAsync(data);
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = await controller.UpdateCountryTaxBands(data);

            //assert
            Assert.NotNull(result);
            var okResponse = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResponse.StatusCode);
            Assert.Equal(data, okResponse.Value);

        }

        [Fact]
        public async Task UpdateCountryTaxBands_Fail()
        {
            var data = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
                Country = "Test",
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

            mockTaxBandService.Setup(x => x.Update(It.IsAny<CountryTaxBandsModel>())).ReturnsAsync(Result.Fail("Something went wrong."));
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = await controller.UpdateCountryTaxBands(data);

            //assert
            Assert.NotNull(result);
            var brResponse = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400,brResponse.StatusCode);
            Assert.NotNull(brResponse.Value);
        }

        [Fact]
        public async Task DeleteCountryTaxBands_OK()
        {
            //arrange
            int id = 1;
            mockTaxBandService.Setup(x => x.Remove(id)).ReturnsAsync(true);
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = await controller.DeleteCountryTaxBands(id);

            //assert
            Assert.NotNull(result );
            var okResponse = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteCountryTaxBands_Fail()
        {
            //arrange
            int id = 1;
            mockTaxBandService.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsync(false);
            var controller = new TaxBandsController(mockTaxBandService.Object, mockLogger.Object);

            //act
            var result = await controller.DeleteCountryTaxBands(id);

            //assert
            Assert.NotNull(result);
            var okResponse = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, okResponse.StatusCode);
        }
    }
}
