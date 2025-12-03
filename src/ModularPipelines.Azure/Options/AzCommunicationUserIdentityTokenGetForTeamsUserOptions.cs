using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "user-identity", "token", "get-for-teams-user")]
public record AzCommunicationUserIdentityTokenGetForTeamsUserOptions(
[property: CliOption("--aad-token")] string AadToken,
[property: CliOption("--aad-user")] string AadUser,
[property: CliOption("--client")] string Client
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}