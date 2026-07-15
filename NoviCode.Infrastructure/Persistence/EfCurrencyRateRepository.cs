using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoviCode
{
    public class EfCurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly AppDbContext _db;

        public EfCurrencyRateRepository(AppDbContext db) => _db = db;

        public async Task SaveRatesAsync(IEnumerable<CurrencyRateDto> rates, CancellationToken cancellationToken = default)
        {
            var list = rates.ToList();
            if (!list.Any()) return;

            var date = list.First().Date.Date;

            var existing = _db.CurrencyRates.Where(r => r.Date == date).Select(r => r.Currency).ToHashSet();

            var toAdd = list
                .Where(d => !existing.Contains(d.Currency))
                .Select(d => new CurrencyRate
                {
                    Id = System.Guid.NewGuid(),
                    Currency = d.Currency,
                    Rate = d.Rate,
                    Date = d.Date.Date
                })
                .ToList();

            if (!toAdd.Any()) return;

            await _db.CurrencyRates.AddRangeAsync(toAdd, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
