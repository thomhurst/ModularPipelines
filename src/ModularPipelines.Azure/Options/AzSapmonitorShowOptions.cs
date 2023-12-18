using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sapmonitor", "show")]
public record AzSapmonitorShowOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;