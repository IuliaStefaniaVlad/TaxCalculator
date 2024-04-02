using FluentResults;
using Microsoft.AspNetCore.Components;
using TaxCalculator.Client.Interfaces;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Client.Components.Pages
{
    public partial class AdminMenu
    {
        //NOTE: This is how to inject in a service in code behind - DI with properties not constructor
        [Inject]
        ITaxBandsClient taxBandsClient { get; set; }
        [Inject]
        ILocalStorageService storageService { get; set; }
        public bool isCreate = false;
        public bool isEdit = false;
        bool hidden = true;
        string error = string.Empty;
        CountryTaxBandsModel? newCountryModel;
        CountryTaxBandsModel? selectedCountry;
        List<CountryTaxBandsModel> countryTaxBandsModels = [];

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //NOTE: this is 100% not cool as session storage can be edited by the end user using browser tools, however, for the sake of the app:
            // It needs to be called AFTER the component is rendered so that JS can actually run
            var token = await storageService.GetItem<LoginResultModel>("login");

            if (token is null)
                _navigationManager.NavigateTo("/login");

            
            var result = await taxBandsClient.GetAllTaxBands();
            if (result.IsSuccess)
            {
                countryTaxBandsModels = result.Value.ToList();
            }
        }
        public void AddTaxBand() {
            if(newCountryModel is not null)
            {
                var order = newCountryModel.TaxBands.Count + 1;
                newCountryModel.TaxBands.Add(new TaxBandModel() { BandOrder = order});
            }
        }
        public void RemoveTaxBand(TaxBandModel taxBandModel)
        {
            if (newCountryModel is not null)
            {
                var index = 1;
                newCountryModel.TaxBands.Remove(taxBandModel);
                foreach(var band in newCountryModel.TaxBands)
                {
                    band.BandOrder = index;
                    index++;
                }
            }
        }
        public void CreateTaxBands() 
        {
            newCountryModel = new CountryTaxBandsModel() { Country = "", TaxBands = [new TaxBandModel() { BandOrder = 1}, new TaxBandModel() { BandOrder = 2}] };
            selectedCountry = null;
            isCreate = true;
            isEdit = false;
        }

        public void EditTaxBands()
        {
            newCountryModel = selectedCountry;
            selectedCountry = null;
            isEdit = true;
            isCreate = false;
        }


        public async void SaveTaxBands()
        {
            hidden = true;
            error = "";
            if (newCountryModel is null)
            {
               
                return;
            }
            if(string.IsNullOrWhiteSpace(newCountryModel.Country))
            {
                hidden = false;
                error = "Country name should not be empty...";
                return;
            }
            Result<CountryTaxBandsModel> result = null;
            if (isCreate)
            {
                result = await taxBandsClient.CreateTaxBands(newCountryModel);
                
            }
            if(isEdit)
            {
                result = await taxBandsClient.UpdateTaxBands(newCountryModel);

            }
          
            if (result.IsFailed)
            {
                hidden = false;
                error = string.Join(",",result.Errors);
            }
            if(result.IsSuccess) {
                selectedCountry = newCountryModel;
                isCreate = false;
                isEdit = false;
            }

            StateHasChanged();


        }
        public async void DeleteTaxBands()
        {
            if(selectedCountry is not null)
            {
                var result = await taxBandsClient.DeleteTaxBands(selectedCountry.CountryTaxBandsModelId!.Value);
                if (result.IsSuccess)
                {
                    countryTaxBandsModels.Remove(selectedCountry);
                    selectedCountry = null;
                    StateHasChanged();

                }

            }
        }
    }
}