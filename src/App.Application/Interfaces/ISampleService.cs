using App.Application.DTOs;

namespace App.Application.Interfaces;

/// <summary>
/// Sample application service interface.
/// </summary>
public interface ISampleService
{
    /// <summary>
    /// Gets all sample items.
    /// </summary>
    /// <returns>Collection of sample DTOs.</returns>
    Task<IEnumerable<SampleDto>> GetSamplesAsync();

    /// <summary>
    /// Performs a health check on the external API.
    /// </summary>
    /// <returns>True if API is reachable; otherwise, false.</returns>
    Task<bool> CheckApiHealthAsync();
}
