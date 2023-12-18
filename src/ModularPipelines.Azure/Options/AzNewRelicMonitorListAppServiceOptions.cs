using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "monitor", "list-app-service")]
public record AzNewRelicMonitorListAppServiceOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--user-email")] string UserEmail
) : AzOptions
{
    [CommandSwitch("--azure-resource-ids")]
    public string? AzureResourceIds { get; set; }
}