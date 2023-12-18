using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "profile", "list")]
public record AzMonitorAutoscaleProfileListOptions(
[property: CommandSwitch("--autoscale-name")] string AutoscaleName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;