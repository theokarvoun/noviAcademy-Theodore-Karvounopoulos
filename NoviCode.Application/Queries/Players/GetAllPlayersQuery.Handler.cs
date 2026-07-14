using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Queries.Players
{
    public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, IReadOnlyList<Player>>
    {
        private readonly IPlayerReadPersistence _persistence;

        public GetAllPlayersQueryHandler(IPlayerReadPersistence persistence)
        {
            _persistence = persistence;
        }

        public Task<IReadOnlyList<Player>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
            => _persistence.GetAll();
    }
}
