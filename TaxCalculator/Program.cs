using ErrorHandlingSample;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaxCalculator.Models.Shared.Options;
using TaxCalculator.Repositories.DBContext;
using TaxCalculator.Repositories.Extensions;
using TaxCalculator.Services.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<JWTOptions>().BindConfiguration("JWT").ValidateDataAnnotations().ValidateOnStart();

builder.Services.AddRepositoryServices(builder.Configuration["AzureDBCString"]);
builder.Services.AddRepositoryMappings();
builder.Services.AddServices();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });


builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(o => { }); // Works -- https://github.com/dotnet/aspnetcore/issues/51888 



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
