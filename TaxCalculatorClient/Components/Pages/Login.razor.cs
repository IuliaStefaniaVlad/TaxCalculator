using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaxCalculator.Client.Interfaces;
using TaxCalculator.Models.Shared;
namespace TaxCalculator.Client.Components.Pages
{
    public partial class Login
    {
        [Inject]
        ILoginClient loginClient { get; set; }
        [Inject]
        ILocalStorageService storageService{ get; set; }
        bool success;
        MudTextField<string> username;
        MudTextField<string> pwField;

        bool hidden = true;
        string error = "";
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //NOTE: If already logged in, move to menu
            var result = await storageService.GetItem<LoginResultModel>("login");
            
            if (result is not null)
            {
                _navigationManager.NavigateTo("/AdminMenu");

            }

        }
        private async Task LoginUser()
        {
            if(success)
            {
                hidden = true;
                error = "";
                var inputModel = new UserInputModel() { Name = username.Text, Password = pwField.Text };
                var result = await loginClient.Login(inputModel);
                if(result.IsSuccess)
                {
                    await storageService.SetItem("login", result.Value);
                    _navigationManager.NavigateTo("/AdminMenu");
                }
                else
                {
                    hidden = false;
                    error = "Invalid user or password";
                }

            }
        
        }

    }
}