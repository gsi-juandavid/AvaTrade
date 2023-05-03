using Hangfire;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AvaTrade.Microservices.NewsCollectionService
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureHangfire(this IServiceCollection services)
        {
            // Configure MongoDB storage for Hangfire
            // TODO: Create and configure a MongoDB database, update this settings
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("Hangfire");
            services.AddHangfire(config =>
                //config.UseMongoStorage(mongoDatabase, new MongoStorageOptions
                //{
                //    Prefix = "hangfire:"
                //})
                config.UseMongoStorage(mongoClient, "Hangfire", new MongoStorageOptions { Prefix = "hangfire:"})
            );

            // Add the Hangfire server
            services.AddHangfireServer();
        }

        public static void ConfigureHangfireDashboard(this IApplicationBuilder app)
        {
            // Use Hangfire Dashboard
            app.UseHangfireDashboard();
        }
    }
}
