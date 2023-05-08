using System;

namespace AvaTrade.Microservices.DataStorageService
{
    public class DbOptions
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
