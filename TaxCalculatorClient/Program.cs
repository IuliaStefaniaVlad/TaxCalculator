using MudBlazor.Services;
using TaxCalculator.Client;
using TaxCalculator.Client.Components;
using TaxCalculator.Client.Interfaces;
using TaxCalculator.Models.Shared.Options;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices()
                .AddRazorPages();


builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<ITaxBandsClient,TaxBandsClient>();
builder.Services.AddScoped<ITaxCalculatorClient,TaxCalculatorClient>();
builder.Services.AddScoped<ILoginClient, LoginClient>();
builder.Services.AddOptions<UriOptions>().BindConfiguration("Uri").ValidateDataAnnotations().ValidateOnStart();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
