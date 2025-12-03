using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic", "monitor", "create")]
public record AzElasticMonitorCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--generate-api-key")]
    public bool? GenerateApiKey { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--monitoring-status")]
    public string? MonitoringStatus { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-info")]
    public string? UserInfo { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}