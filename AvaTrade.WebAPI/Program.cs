using AvaTrade.Microservices.DataStorageService;
using AvaTrade.Microservices.NewsCollectionService;
using AvaTrade.Microservices.NewsCollectionService.Jobs;
using AvaTrade.Microservices.NewsCollectionService.Polygon.io;
using AvaTrade.WebAPI.Authentication;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

//*** Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the authentication options from the appsettings.json file
AuthenticationIssuerOptions authOptions = new AuthenticationIssuerOptions();
builder.Configuration.GetSection("AuthenticationIssuer").Bind(authOptions);
builder.Services.ConfigureAuthentication(authOptions);

// Get the Polygon.io options from the appsettings.json file
PolygonOptions polygonOptions = new PolygonOptions();
builder.Configuration.GetSection("Polygon").Bind(polygonOptions);
builder.Services.AddSingleton(polygonOptions);

// Get the main DB connection string from the appsettings.json file
DbOptions dbConnection = new DbOptions();
builder.Configuration.GetSection("NewsDatabase").Bind(dbConnection);
builder.Services.AddSingleton(dbConnection);

builder.Services.ConfigureHangfire();
builder.Services.AddDataServices();
builder.Services.AddLogging(loggerBuilder =>
{
    loggerBuilder.ClearProviders();
    loggerBuilder.AddConsole();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureHangfireDashboard();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

//Add hanfire jobs
RecurringJob.AddOrUpdate<PolygonNewsCollectionJob>("PolygonNewsCollectionJob", x => x.ExecuteAsync(), Cron.Hourly);

app.Run();
