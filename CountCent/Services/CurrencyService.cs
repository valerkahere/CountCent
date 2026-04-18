using System.Text.Json;
using CountCent.Model;

namespace CountCent.Services
{
    // API JSON map
    public class ExchangeRates
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        // DI injects client
        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertAmountAsync(decimal localAmount, string targetCurrency = "USD")
        {
            try
            {
                // Fetch rates (default base usually EUR)
                var response = await _httpClient.GetAsync("latest");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ExchangeRates>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Map to db scale
                if (data?.Rates != null && data.Rates.TryGetValue(targetCurrency.ToUpper(), out decimal rate))
                {
                    return localAmount * rate;
                }

                return localAmount; // Fallback 1:1 if currency not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API fail: {ex.Message}");
                return localAmount; // Fallback 1:1 if network down
            }
        }
        
        // Convert existing DB models in batch
        public async Task<List<DataPoint>> ConvertDataPointsAsync(List<DataPoint> points, string targetCurrency)
        {
            // Fetch once. Apply to all. Save API calls.
            var response = await _httpClient.GetAsync("latest");
            if (!response.IsSuccessStatusCode) return points;
            
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ExchangeRates>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (data?.Rates == null || !data.Rates.TryGetValue(targetCurrency.ToUpper(), out decimal rate)) 
                return points;

            // Map new values to copy of models
            return points.Select(p => new DataPoint(p.Amount * rate, p.Date) { Id = p.Id }).ToList();
        }
    }
}