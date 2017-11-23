using BigData.Hubs;
using BigData.Models;
using BigData.Storage;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BigData.Services
{
    public class CurrencyRateTicker
    {
        private static readonly CurrencyRateTicker instance_;
        private static readonly object locker_ = new object();

        private readonly ICurrencyRateStorage storage_;
        private readonly TimeSpan updateInterval_;
        private readonly Random random_;
        private readonly Timer timer_;

        static CurrencyRateTicker()
        {
            // TODO: do not make a singleton, use DI
            instance_ = new CurrencyRateTicker(
                GlobalHost.ConnectionManager.GetHubContext<CurrencyRateHub>().Clients,
                new RedisCurrencyRateStorage(RedisConnectionManager.Instance)
            );
        }

        private CurrencyRateTicker(IHubConnectionContext<dynamic> clients, ICurrencyRateStorage storage)
        {
            storage_ = storage;
            updateInterval_ = TimeSpan.FromMilliseconds(250);
            random_ = new Random();
            Clients = clients;

            // TODO: get rates from redis, store to redis
            timer_ = new Timer(OnTimerElapsed, null, TimeSpan.Zero, updateInterval_);
        }

        public static void Start()
        {
            // referencing is enough to launch the static constuctor

            // TODO: implement an approach without singletons
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        private List<CurrencyRate> GetInitialRates()
        {
            return new List<CurrencyRate>
            {
                new CurrencyRate {Name = "USD", Rate = 1.98m},
                new CurrencyRate {Name = "EUR", Rate = 2.2m},
                new CurrencyRate {Name = "RUB (x100)", Rate = 3.3m},
            };
        }

        private async void OnTimerElapsed(object state)
        {
            var rates = (await storage_.GetCurrentRates()).ToList();
            if (!rates.Any())
            {
                rates = GetInitialRates();
            }
            var newRates = UpdateRates(rates);
            // broadcast
            Clients.All.updateCurrencyRates(newRates);
            await storage_.UpdateRates(newRates);
        }

        private List<CurrencyRate> UpdateRates(List<CurrencyRate> rates)
        {
            decimal maxPercentage = 1m;
            foreach (var rate in rates)
            {
                if (random_.NextDouble() > 0.15)
                {
                    // do not update
                    continue;
                }
                decimal delta = (decimal)random_.NextDouble() * maxPercentage;
                if (random_.Next(2) > 0)
                {
                    delta = -delta;
                }
                rate.Rate = rate.Rate * (1 + delta/100);
            }
            return rates;
        }
    }
}