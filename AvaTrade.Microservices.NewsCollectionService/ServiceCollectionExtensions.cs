﻿using AvaTrade.Microservices.DataStorageService;
using AvaTrade.Microservices.DataStorageService.ServiceContracts;
using AvaTrade.Microservices.DataStorageService.Services;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
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
            var dbOptions = services.BuildServiceProvider().GetRequiredService<DbOptions>();
            var mongoClient = new MongoClient(dbOptions.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbOptions.DatabaseName);

            var storageOptions = new MongoStorageOptions
            {
                Prefix = "hangfire",
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new DropMongoMigrationStrategy()
                },
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
            };

            services.AddHangfire(config => config.UseMongoStorage(mongoClient, dbOptions.DatabaseName, storageOptions));

            // Add the Hangfire server
            services.AddHangfireServer();
        }

        public static void ConfigureHangfireDashboard(this IApplicationBuilder app)
        {
            // Use Hangfire Dashboard
            app.UseHangfireDashboard();
        }

        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<INewsService, NewsService>();
        }
    }
}
