using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workloads", "monitor", "provider-instance", "create")]
public record AzWorkloadsMonitorProviderInstanceCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provider-settings")]
    public string? ProviderSettings { get; set; }
}