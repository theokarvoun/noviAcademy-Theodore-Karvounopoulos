using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Queries.Players
{
    public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Player?>
    {
        private readonly IPlayerReadPersistence _persistence;

        public GetPlayerByIdQueryHandler(IPlayerReadPersistence persistence)
        {
            _persistence = persistence;
        }

        // No caching/logging here — those are layered by decorators.
        public Task<Player?> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
            => _persistence.GetById(request.Id);
    }
}
