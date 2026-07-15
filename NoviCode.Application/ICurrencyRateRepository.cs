using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NoviCode
{
    public interface ICurrencyRateRepository
    {
        Task SaveRatesAsync(IEnumerable<CurrencyRateDto> rates, CancellationToken cancellationToken = default);
    }
}
