namespace App.Domain.Entities;

/// <summary>
/// Represents a user setting persisted in the database.
/// </summary>
public class UserSetting : BaseEntity
{
    /// <summary>
    /// The key of the setting.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The value of the setting.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
