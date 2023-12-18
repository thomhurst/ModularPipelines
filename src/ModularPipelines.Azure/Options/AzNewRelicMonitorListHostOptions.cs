using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "monitor", "list-host")]
public record AzNewRelicMonitorListHostOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--user-email")] string UserEmail
) : AzOptions
{
    [CommandSwitch("--vm-ids")]
    public string? VmIds { get; set; }
}