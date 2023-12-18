using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "entity-query", "create")]
public record AzSentinelEntityQueryCreateOptions(
[property: CommandSwitch("--entity-query-id")] string EntityQueryId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--activity")]
    public string? Activity { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}