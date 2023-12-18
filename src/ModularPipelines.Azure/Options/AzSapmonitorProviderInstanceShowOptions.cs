using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sapmonitor", "provider-instance", "show")]
public record AzSapmonitorProviderInstanceShowOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--provider-instance-name")] string ProviderInstanceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}

