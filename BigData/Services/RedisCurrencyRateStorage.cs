using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BigData.Models;
using BigData.Storage;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Linq;

namespace BigData.Services
{
    public class RedisCurrencyRateStorage : ICurrencyRateStorage
    {
        private readonly IRedisConnectionManager manager_;

        public RedisCurrencyRateStorage(IRedisConnectionManager manager)
        {
            manager_ = manager;
        }

        public async Task<IEnumerable<CurrencyRate>> GetCurrentRates()
        {
            var db = manager_.GetDatabase();

            var rates = (await db.ListRangeAsync(Key));
            return rates.Select(x => JsonConvert.DeserializeObject<CurrencyRate>(x));
        }

        public Task UpdateRates(IEnumerable<CurrencyRate> rates)
        {
            var db = manager_.GetDatabase();

            var transaction = db.CreateTransaction();
            transaction.KeyDeleteAsync(Key);
            var serialized = rates.Select(rate => (RedisValue)JsonConvert.SerializeObject(rate)).ToArray();
            transaction.ListRightPushAsync(Key, serialized);
            return transaction.ExecuteAsync();
        }

        private RedisKey Key => "rates";
    }
}
