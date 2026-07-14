namespace NoviCode.Infrastructure
{
    // Read-side persistence port for players (the Query side of CQRS).
    // The Application declares WHAT it needs to read; Infrastructure decides HOW
    // (EF Core here) and a decorator layers caching on top.
    public interface IPlayerReadPersistence
    {
        Task<Player?> GetById(Guid id);
        Task<IReadOnlyList<Player>> GetAll();
    }
}
