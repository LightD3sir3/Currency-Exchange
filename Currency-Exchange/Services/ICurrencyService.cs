using Currency_Exchange.Models;

namespace Services
{
    public interface ICurrencyService
    {
        public Task<decimal> GetExchangeRate(string baseCurrency, string targetCurrency);

        Task<List<Currency>> GetAllCurrencies();
    }
}
