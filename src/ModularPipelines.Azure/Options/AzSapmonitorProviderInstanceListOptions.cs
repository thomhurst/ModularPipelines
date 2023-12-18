using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sapmonitor", "provider-instance", "list")]
public record AzSapmonitorProviderInstanceListOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;