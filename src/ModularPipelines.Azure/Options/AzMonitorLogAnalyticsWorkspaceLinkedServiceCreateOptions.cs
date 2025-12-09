using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "workspace", "linked-service", "create")]
public record AzMonitorLogAnalyticsWorkspaceLinkedServiceCreateOptions(
[property: CliOption("--linked-service-name")] string LinkedServiceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--write-access-resource-id")]
    public string? WriteAccessResourceId { get; set; }
}