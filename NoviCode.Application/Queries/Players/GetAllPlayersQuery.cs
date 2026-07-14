using MediatR;

namespace NoviCode.Queries.Players
{
    public record GetAllPlayersQuery() : IRequest<IReadOnlyList<Player>>;
}
