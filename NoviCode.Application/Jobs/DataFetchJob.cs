using MediatR;
using Quartz;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Threading;

using NoviCode;

namespace NoviCode.Jobs
{
    [DisallowConcurrentExecution]
    public class DataFetchJob : IJob
    {

        private readonly IEcbHttpClient _client;
        private readonly ICurrencyRateRepository _repository;

        public DataFetchJob(IEcbHttpClient client, ICurrencyRateRepository repository)
        {
            _client = client;
            _repository = repository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dtoresults = await _client.GetLatestRatesAsync(context.CancellationToken);

            // Map DTOs to EF entities and persist if not already present for the same date+currency
            // Persist via repository (Infrastructure implementation will handle duplicates)
            await _repository.SaveRatesAsync(dtoresults, context.CancellationToken);
        }
    }
}
