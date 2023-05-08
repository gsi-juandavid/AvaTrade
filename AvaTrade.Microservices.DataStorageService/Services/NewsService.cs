﻿using AvaTrade.Microservices.DataStorageService.Models;
using AvaTrade.Microservices.DataStorageService.ServiceContracts;
using MongoDB.Driver;

namespace AvaTrade.Microservices.DataStorageService.Services;

public class NewsService : INewsService
{
    private readonly IMongoCollection<NewsItem> _newsCollection;

    public NewsService(DbOptions dbOptions)
    {
        var mongoClient = new MongoClient(
            dbOptions.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbOptions.DatabaseName);

        _newsCollection = mongoDatabase.GetCollection<NewsItem>(
            dbOptions.NewsCollectionName);
    }

    public async Task<List<NewsItem>> GetAsync() =>
        await _newsCollection.Find(_ => true).ToListAsync();

    public async Task<NewsItem?> GetAsync(string id) =>
        await _newsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(NewsItem newNewsItem) =>
        await _newsCollection.InsertOneAsync(newNewsItem);

    public async Task UpdateAsync(string id, NewsItem updatedNewsItem) =>
        await _newsCollection.ReplaceOneAsync(x => x.Id == id, updatedNewsItem);

    public async Task RemoveAsync(string id) =>
        await _newsCollection.DeleteOneAsync(x => x.Id == id);
}
