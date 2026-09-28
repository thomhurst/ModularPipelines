using System.Text;

namespace ModularPipelines.Distributed;

/// <summary>
/// The endpoint a distributed master exposes to its workers.
/// </summary>
public sealed record MasterEndpoint
{
    /// <summary>Gets the absolute URL workers connect to.</summary>
    public required Uri Url { get; init; }

    /// <summary>
    /// Gets the access token workers present to the master, when the master requires one.
    /// The token is redacted from <see cref="ToString"/>.
    /// </summary>
    public string? AccessToken { get; init; }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Url = ").Append(Url);
        builder.Append(", AccessToken = ").Append(AccessToken is null ? "<none>" : "<redacted>");
        return true;
    }
}
