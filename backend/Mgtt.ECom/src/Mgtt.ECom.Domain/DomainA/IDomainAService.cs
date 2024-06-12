namespace Mgtt.ECom.Domain.DomainA;

/// <summary>
/// Represents a service interface for managing domain model A.
/// </summary>
public interface IDomainAService
{
    /// <summary>
    /// Creates new domain model A asynchronously.
    /// </summary>
    /// <param name="domainModelA">The domain model A to create.</param>
    /// <returns>The created domain model A.</returns>
    Task<DomainModelA?> CreateAsync(DomainModelA? domainModelA);

    /// <summary>
    /// Gets DomainA based on specified criteria asynchronously.
    /// </summary>
    /// <param name="filter">The criteria for filtering DomainA (optional).</param>
    /// <returns>A collection of DomainA matching the specified criteria.</returns>
    Task<IEnumerable<DomainModelA>?> GetAsync(DomainAFilter? filter);

    /// <summary>
    /// Gets DomainA by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the domain model A.</param>
    /// <returns>The domain model A with the specified identifier.</returns>
    Task<DomainModelA?> GetByIdAsync(Guid id);

    /// <summary>
    /// Updates existing DomainA by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the domain model A to update.</param>
    /// <param name="domainModelA">The new domain model A.</param>
    /// <returns>The updated domain model A.</returns>
    Task<DomainModelA?> UpdateByIdAsync(Guid id, DomainModelA? domainModelA);

    /// <summary>
    /// Deletes DomainA by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the domain model A to delete.</param>
    /// <returns>The deleted domain model A.</returns>
    Task<DomainModelA?> DeleteByIdAsync(Guid id);
}
