using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "user-identity", "token", "revoke")]
public record AzCommunicationUserIdentityTokenRevokeOptions(
[property: CliOption("--user")] string User
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}