using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "user-identity", "user", "create")]
public record AzCommunicationUserIdentityUserCreateOptions : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}