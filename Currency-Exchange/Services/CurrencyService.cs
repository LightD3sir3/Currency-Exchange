using Currency_Exchange.Models;
using System.Text.Json;

namespace Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetExchangeRate(string baseCurrency, string targetCurrency)
        {
            var response = await _httpClient.GetFromJsonAsync<JsonDocument>($"https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/{baseCurrency.ToLower()}.json");
            return response.RootElement.GetProperty(baseCurrency.ToLower()).GetProperty(targetCurrency.ToLower()).GetDecimal();
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>("https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies.json");

            var currencies = new List<Currency>();
            foreach (var item in response)
            {
                currencies.Add(new Currency { Code = item.Key, Name = item.Value });
            }

            return currencies;
        }
    }
}
