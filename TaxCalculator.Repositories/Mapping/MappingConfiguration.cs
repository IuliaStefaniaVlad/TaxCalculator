using Mapster;
using TaxCalculator.Models.Shared;
using TaxCalculator.Repositories.Entities;

namespace TaxCalculator.Repositories.Mapping;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config?.NewConfig<CountryTaxBandsEntity, CountryTaxBandsModel>()
            .Map(dst => dst.CountryTaxBandsModelId, src => src.CountryTaxBandsId);

        //looks like I had to write it twice, .TwoWays() was not enough
        config?.NewConfig<CountryTaxBandsModel, CountryTaxBandsEntity>()
            .Map(dst => dst.CountryTaxBandsId, src => src.CountryTaxBandsModelId);

        config?.NewConfig<TaxBandEntity, TaxBandModel>().TwoWays();

    }
}
