namespace WorldRank.Application.Interfaces;

/// <summary>
/// Application-owned cache abstraction (a "port"). The Application layer declares WHAT it
/// needs from a cache; it does not know or care HOW the cache is implemented. The concrete
/// technology (in-memory, Redis, ...) lives in Infrastructure and implements this interface.
///
/// This is the Clean Architecture rule: the Application never references
/// Microsoft.Extensions.Caching.Memory. Dependencies point inward, toward an abstraction the
/// inner layer owns, so caching stays a swappable infrastructure detail.
/// </summary>
public interface ICache
{
	/// <summary>Try to read a cached value. Returns true and sets <paramref name="value"/> on a hit; false on a miss.</summary>
	bool TryGet<T>(string key, out T? value);

	/// <summary>Store a value under <paramref name="key"/> with a time-to-live.</summary>
	void Set<T>(string key, T value, TimeSpan ttl);

	/// <summary>Evict an entry (used to invalidate list views after a write).</summary>
	void Remove(string key);
}
