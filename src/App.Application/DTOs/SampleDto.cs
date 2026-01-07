namespace App.Application.DTOs;

/// <summary>
/// Data Transfer Object for sample data.
/// </summary>
public class SampleDto
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the sample item.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the sample item.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
