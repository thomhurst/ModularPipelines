using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("new-relic", "monitor", "list-app-service")]
public record AzNewRelicMonitorListAppServiceOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--user-email")] string UserEmail
) : AzOptions
{
    [CliOption("--azure-resource-ids")]
    public string? AzureResourceIds { get; set; }
}