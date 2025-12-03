using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("new-relic", "monitor", "list-host")]
public record AzNewRelicMonitorListHostOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--user-email")] string UserEmail
) : AzOptions
{
    [CliOption("--vm-ids")]
    public string? VmIds { get; set; }
}