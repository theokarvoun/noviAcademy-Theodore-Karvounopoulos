using MediatR;

namespace NoviCode.Queries.Players
{
    public record GetPlayerByIdQuery(Guid Id) : IRequest<Player?>;
}
