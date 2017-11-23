using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using BigData.Models;
using System.Threading.Tasks;
using BigData.Services;
using BigData.Storage;

namespace BigData.Hubs
{
    public class CurrencyRateHub : Hub
    {
        private readonly ICurrencyRateStorage storage_;

        public CurrencyRateHub()
        {
            // TODO: set up DI
            storage_ = new RedisCurrencyRateStorage(RedisConnectionManager.Instance);
        }

        public Task<IEnumerable<CurrencyRate>> GetCurrencyRates()
        {
            return storage_.GetCurrentRates();
        }

        public void OnArticleCreated()
        {
            Clients.All.onArticleCreated();
        }
    }
}
