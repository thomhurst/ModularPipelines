using System.Text;
using ModularPipelines.Secrets;

namespace ModularPipelines.Options;

/// <summary>
/// Credentials used when starting a command-line process.
/// </summary>
/// <remarks>The password is redacted when printing this record.</remarks>
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
    [SecretValue]
    public string? Password { get; init; }

    /// <summary>
    /// Gets a value indicating whether the user's profile should be loaded.
    /// </summary>
    public bool LoadUserProfile { get; init; }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Domain = ").Append(Domain);
        builder.Append(", UserName = ").Append(UserName);
        builder.Append(", Password = ").Append(Password is null ? "<none>" : "<redacted>");
        builder.Append(", LoadUserProfile = ").Append(LoadUserProfile);
        return true;
    }
}
