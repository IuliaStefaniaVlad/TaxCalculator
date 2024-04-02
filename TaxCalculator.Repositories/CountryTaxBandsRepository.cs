using FluentResults;
using MapsterMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaxCalculator.Models.Shared;
using TaxCalculator.Repositories.DBContext;
using TaxCalculator.Repositories.Entities;
using TaxCalculator.Repositories.Interfaces;

namespace TaxCalculator.Repositories;


/// <summary>
/// This approach for repository pattern is a good fit for this situation, although I "handle" two different entities.
/// The reason for this is that I don't actually allow the interaction with the TaxBandEntity, just with the CountryTaxBands
/// Since I don't want to allow the individual CRUD operations on the TaxBandEntity it is safe to have a repository that handles the CountryTaxBands without an additional layer of 
/// Unit of work
///
/// </summary>
public class CountryTaxBandsRepository : ICountryTaxBandsRepository
{
    private readonly CountryTaxBandsDbContext _dbContext;
    private readonly IMapper _mapper;

    public CountryTaxBandsRepository(CountryTaxBandsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<Result<CountryTaxBandsModel>> Add(CountryTaxBandsModel countryTaxBands)
    {

        try
        {
            var entity = _mapper.Map<CountryTaxBandsEntity>(countryTaxBands);

            _dbContext.CountryTaxBands.Add(entity);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<CountryTaxBandsModel>(entity);
            }
            return Result.Fail("Something went wrong during insertion, we'll try to figure it out");
        }
        catch (DbUpdateException ex)
        {
            // https://stackoverflow.com/questions/3967140/duplicate-key-exception-from-entity-framework
            SqlException innerException = ex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
            {

                return Result.Fail($"Looks like a duplicate entry, maybe you already set up the tax bands for {countryTaxBands.Country} ");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            //This may not be the best approach, to simply return the ex.Message as I usually don't want to expose sensitive data
            //There should be multiple catches for the exceptions expected to see (already exists maybe for country) 
            // and treat each one accordingly. Same goes for all catches in this class
            return Result.Fail($"Something went wrong during insertion {ex.Message}");
        }

    }

    // to retrive all... pretty much need to Map from entites to models :D
    public Result<ICollection<CountryTaxBandsModel>> GetAll()
    {
        return Result.Ok(_mapper.Map<ICollection<CountryTaxBandsModel>>(_dbContext.CountryTaxBands.Include(x => x.TaxBands)));
    }

    public Result<CountryTaxBandsModel> GetByCountry(string country)
    {
        //if this throws an error.. it should be handled in the middleware as it should be a 500
        var result = Find(x => x.Country == country).SingleOrDefault();
        if (result is null)
        {
            return Result.Fail("We didn't find the required data");
        }
        return _mapper.Map<CountryTaxBandsModel>(result);

    }


    public async Task<bool> Remove(int id)
    {
        try
        {
            var entity = Find(x => x.CountryTaxBandsId == id).Single();
            _dbContext.CountryTaxBands.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Result<CountryTaxBandsModel>> Update(CountryTaxBandsModel countryTaxBands)
    {

        try
        {
            var entity = _mapper.Map<CountryTaxBandsEntity>(countryTaxBands);
            var existingEntity = Find(x => x.CountryTaxBandsId == entity.CountryTaxBandsId).Single();
            
            existingEntity.Country = countryTaxBands.Country;

            existingEntity.TaxBands.Clear();

            foreach (var taxBand in entity.TaxBands)
            {
                existingEntity.TaxBands.Add(taxBand);
            }

            if ( await _dbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<CountryTaxBandsModel>(entity);
            }
            return Result.Fail("Something went wrong when we tried to update the data :(");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Something went wrong when we tried to update the data :( {ex.Message}");
        }

    }

    private IEnumerable<CountryTaxBandsEntity> Find(Expression<Func<CountryTaxBandsEntity, bool>> predicate)
    {
        return _dbContext.CountryTaxBands.Where(predicate).Include(nameof(CountryTaxBandsEntity.TaxBands));
    }
}
