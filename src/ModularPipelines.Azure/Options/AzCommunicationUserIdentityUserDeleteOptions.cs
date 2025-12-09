using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "user-identity", "user", "delete")]
public record AzCommunicationUserIdentityUserDeleteOptions(
[property: CliOption("--user")] string User
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}