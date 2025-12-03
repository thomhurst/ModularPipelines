using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "user-identity", "token", "issue")]
public record AzCommunicationUserIdentityTokenIssueOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}