using TaxCalculator.Models.Shared;

namespace TaxCalculator.Services.Tests
{
    public class TaxBandsValidationHelperTests
    {
        
        [Fact]
        public void ValidateTaxBandInput_OK()
        {
            //arrange
            ICollection<TaxBandModel> taxBands = new List<TaxBandModel>()
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

            //act
            var result = TaxBandsValidationHelper.ValidateTaxBandInput(taxBands);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ValidateTaxBandInput_NullInput()
        {
            //act
            var result = TaxBandsValidationHelper.ValidateTaxBandInput(null);
            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotNull(result.Errors);

        }

        [Theory]
        [InlineData(-2, 1000, 0)]
        [InlineData(0, 1000, -3)]
        [InlineData(1000, -50, 0)]
        public void ValidateTaxBandInput_NegativeData(int minRange, int maxRange, int rate)
        {
            //arrange
            ICollection<TaxBandModel> taxBands = new List<TaxBandModel>()
            {
                new TaxBandModel()
                {
                    BandOrder = 1,
                    MinRange = minRange,
                    MaxRange = maxRange,
                    TaxRate = rate
                }
            };
            //act
            var result = TaxBandsValidationHelper.ValidateTaxBandInput(taxBands);
            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotNull(result.Errors);
        }

       
        [Theory]
        [ClassData(typeof(InvalidTestDataGenerator))]
        public void ValidateTaxBandInput_InconsistentConsecutiveData(TaxBandModel taxBand1, TaxBandModel taxBand2)
        {
            //arrange
            var taxBands = new List<TaxBandModel>() { taxBand1, taxBand2 };
            //act
            var result = TaxBandsValidationHelper.ValidateTaxBandInput(taxBands);
            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotNull(result.Errors);
        }

        [Fact]
        public void ValidateTaxBandInput_LastBandNotNull()
        {
            //arrange
            ICollection<TaxBandModel> taxBands = new List<TaxBandModel>()
            {
                new TaxBandModel()
                {
                    BandOrder = 1,
                    MinRange = 20,
                    MaxRange = 50000,
                    TaxRate = 20
                }
            };
            //act
            var result = TaxBandsValidationHelper.ValidateTaxBandInput(taxBands);
            //assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.NotNull(result.Errors);
        }
    }
}
