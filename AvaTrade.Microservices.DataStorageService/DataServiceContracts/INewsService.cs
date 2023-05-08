using AvaTrade.Microservices.DataStorageService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaTrade.Microservices.DataStorageService.ServiceContracts
{
    public interface INewsService
    {
        Task<List<NewsItem>> GetAsync();
        Task<NewsItem?> GetAsync(string id);
        Task CreateAsync(NewsItem newNewsItem);
        Task UpdateAsync(string id, NewsItem updatedNewsItem);
        Task RemoveAsync(string id);
    }
}
