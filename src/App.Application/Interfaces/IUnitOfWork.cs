using App.Domain.Entities;
using App.Domain.Interfaces;

namespace App.Application.Interfaces;

/// <summary>
/// Interface for Unit of Work to manage transaction boundaries.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for a specific entity type.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <returns>Repository instance.</returns>
    IRepository<T> Repository<T>() where T : BaseEntity;

    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    /// <returns>Number of affected rows.</returns>
    Task<int> SaveChangesAsync();
}
