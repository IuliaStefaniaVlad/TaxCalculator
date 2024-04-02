using System.Collections;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Services.Tests
{
    public class InvalidTestDataGenerator : IEnumerable<object[]>
    {
        private readonly IEnumerable<object[]> _data = new List<object[]>
        {
            // consecutive band order
            new object[] {
                    new TaxBandModel()
                    {
                        BandOrder = 1,
                        MinRange = 0,
                        MaxRange = 1000,
                        TaxRate = 0
                    },
                    new TaxBandModel()
                    {
                        BandOrder = 3,
                        MinRange = 1000,
                        MaxRange = 50000,
                        TaxRate = 20
                    }
            },
            // consecutive band order
            new object[] {
                    new TaxBandModel()
                    {
                        BandOrder = 2,
                        MinRange = 0,
                        MaxRange = 1000,
                        TaxRate = 0
                    },
                    new TaxBandModel()
                    {
                        BandOrder = 1,
                        MinRange = 1000,
                        MaxRange = 50000,
                        TaxRate = 20
                    }
            },
       
            new object[] {
                    new TaxBandModel()
                    {
                        BandOrder = 1,
                        MinRange = 0,
                        MaxRange = 1000,
                        TaxRate = 0
                    },
                    new TaxBandModel()
                    {
                        BandOrder = 2,
                        MinRange = 2000,
                        MaxRange = 50000,
                        TaxRate = 20
                    }
            }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


}
