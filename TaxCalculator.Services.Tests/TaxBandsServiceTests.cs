using FluentResults;
using Moq;
using TaxCalculator.Models.Shared;
using TaxCalculator.Repositories.Interfaces;

namespace TaxCalculator.Services.Tests
{
    public class TaxBandsServiceTests
    {
        private Mock<ICountryTaxBandsRepository> countryTaxBandsRepository = new Mock<ICountryTaxBandsRepository>();


        [Fact]
        public async Task Add_OK()
        {
            //arrange
            CountryTaxBandsModel inputData = new CountryTaxBandsModel()
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
            };
            var expectedData = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
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
            }; ;

            countryTaxBandsRepository.Setup(x => x.Add(inputData)).ReturnsAsync(expectedData);
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Add(inputData);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var actualResult = Assert.IsType<CountryTaxBandsModel>(result.Value);
            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.CountryTaxBandsModelId);
            Assert.Equal(expectedData, actualResult);

        }

        //is it worth doing this tho?...
        [Theory]
        [ClassData(typeof(InvalidTestDataGenerator))]
        public async Task Add_DataValidationFail(TaxBandModel taxBand1, TaxBandModel taxBand2)
        {
            //arrange
            CountryTaxBandsModel inputData = new CountryTaxBandsModel()
            {
                Country = "England",
                TaxBands = new List<TaxBandModel>(){ taxBand1, taxBand2} 
            };

            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Add(inputData);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task Add_RepoFailure()
        {
            //arrange
            var inputData = new CountryTaxBandsModel()
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
            };

            countryTaxBandsRepository.Setup(x => x.Add(inputData)).ReturnsAsync(Result.Fail("Something went wrong."));
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Add(inputData);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void GetAll_OK()
        {
            ICollection<CountryTaxBandsModel> expectedData = new List<CountryTaxBandsModel>() 
            {
                new CountryTaxBandsModel()
                {
                    CountryTaxBandsModelId = 1,
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
                    CountryTaxBandsModelId = 2,
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
                                        TaxRate = 20
                                    }
                                }
                }
            };
            countryTaxBandsRepository.Setup(x => x.GetAll()).Returns(Result.Ok(expectedData));
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = service.GetAll();

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedData, result.Value);

        }

        [Fact]
        public void GetAll_Fail()
        {
            //ummm pare ca bu poate sa faileze vreodata, ce se intampla daca n-ai nimic in db?
        }

        [Fact]
        public void GetByCountry_OK() 
        {
            //arrange
            var countryName = "England";
            var expectedData = new CountryTaxBandsModel()
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

            countryTaxBandsRepository.Setup(x => x.GetByCountry(countryName)).Returns(expectedData);
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = service.GetByCountry(countryName);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedData, result.Value);
            
        }

        [Fact]
        public void GetByCountry_NotFound()
        {
            //arrange
            countryTaxBandsRepository.Setup(x => x.GetByCountry(It.IsAny<string>())).Returns(Result.Fail("Country not found."));
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = service.GetByCountry("Romania");

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task Remove_OK()
        {
            //arrange
            var id = 1;
            countryTaxBandsRepository.Setup(x => x.Remove(id)).ReturnsAsync(true);
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Remove(id);

            //assert
            Assert.True(result);
        }

        [Fact]
        public async Task Remove_Fail()
        {
            var id = 0;
            countryTaxBandsRepository.Setup(x => x.Remove(id)).ReturnsAsync(false);
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Remove(id);

            //assert
            Assert.False(result);
        }

        [Fact]
        public async Task Update_OK()
        {
            //arrange
            CountryTaxBandsModel data = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
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
            };

            countryTaxBandsRepository.Setup(x => x.Update(data)).ReturnsAsync(data);
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Update(data);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(data, result.Value);
        }

        [Theory]
        [ClassData(typeof(InvalidTestDataGenerator))]
        public async Task Update_DataValidationFail(TaxBandModel taxBand1, TaxBandModel taxBand2)
        {
            //arrange
            CountryTaxBandsModel data = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
                Country = "England",
                TaxBands = new List<TaxBandModel>() { taxBand1, taxBand2 }
            };

            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Update(data);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task Update_RepoFailure()
        {
            //arrange
            CountryTaxBandsModel data = new CountryTaxBandsModel()
            {
                CountryTaxBandsModelId = 1,
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
            };
            countryTaxBandsRepository.Setup(x => x.Update(It.IsAny<CountryTaxBandsModel>())).ReturnsAsync(Result.Fail("Could not update values."));
            var service = new TaxBandsService(countryTaxBandsRepository.Object);

            //act
            var result = await service.Update(data);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
        }
    }
}