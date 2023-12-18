using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "user-identity", "token", "issue")]
public record AzCommunicationUserIdentityTokenIssueOptions(
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}