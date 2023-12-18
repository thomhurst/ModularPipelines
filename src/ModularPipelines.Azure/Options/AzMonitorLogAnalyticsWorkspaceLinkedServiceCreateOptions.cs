using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "linked-service", "create")]
public record AzMonitorLogAnalyticsWorkspaceLinkedServiceCreateOptions(
[property: CommandSwitch("--linked-service-name")] string LinkedServiceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--write-access-resource-id")]
    public string? WriteAccessResourceId { get; set; }
}