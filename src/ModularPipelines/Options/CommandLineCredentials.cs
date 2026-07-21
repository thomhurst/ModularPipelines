namespace ModularPipelines.Options;

/// <summary>
/// Credentials used when starting a command-line process.
/// </summary>
public sealed record CommandLineCredentials
{
    /// <summary>
    /// Gets the domain associated with the user account.
    /// </summary>
    public string? Domain { get; init; }

    /// <summary>
    /// Gets the user name used to start the process.
    /// </summary>
    public string? UserName { get; init; }

    /// <summary>
    /// Gets the password associated with the user account.
    /// </summary>
    public string? Password { get; init; }

    /// <summary>
    /// Gets a value indicating whether the user's profile should be loaded.
    /// </summary>
    public bool LoadUserProfile { get; init; }
}
