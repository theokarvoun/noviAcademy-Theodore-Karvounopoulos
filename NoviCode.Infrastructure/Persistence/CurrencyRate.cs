using System;

namespace NoviCode
{
    public class CurrencyRate
    {
        public Guid Id { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
