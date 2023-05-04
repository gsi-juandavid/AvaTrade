using AvaTrade.Microservices.NewsCollectionService;
using AvaTrade.WebAPI.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.ConfigureHangfire();

// Get the authentication options from the appsettings.json file
AuthenticationIssuerOptions options = new AuthenticationIssuerOptions();
builder.Configuration.GetSection("AuthenticationIssuerOptions").Bind(options);
builder.Services.ConfigureAuthentication(options);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.ConfigureHangfireDashboard();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
