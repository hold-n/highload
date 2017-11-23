using Newtonsoft.Json;

namespace BigData.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }
}