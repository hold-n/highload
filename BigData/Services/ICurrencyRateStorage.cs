using BigData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigData.Services
{
    public interface ICurrencyRateStorage
    {
        Task<IEnumerable<CurrencyRate>> GetCurrentRates();

        Task UpdateRates(IEnumerable<CurrencyRate> rates);
    }
}

