using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "user-identity", "user", "create")]
public record AzCommunicationUserIdentityUserCreateOptions : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}