using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "get-shared-keys")]
public record AzMonitorLogAnalyticsWorkspaceGetSharedKeysOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;