using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "workspace", "pack", "enable")]
public record AzMonitorLogAnalyticsWorkspacePackEnableOptions(
[property: CliOption("--intelligence-pack-name")] string IntelligencePackName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;