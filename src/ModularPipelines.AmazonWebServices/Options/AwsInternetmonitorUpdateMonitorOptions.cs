using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internetmonitor", "update-monitor")]
public record AwsInternetmonitorUpdateMonitorOptions(
[property: CliOption("--monitor-name")] string MonitorName
) : AwsOptions
{
    [CliOption("--resources-to-add")]
    public string[]? ResourcesToAdd { get; set; }

    [CliOption("--resources-to-remove")]
    public string[]? ResourcesToRemove { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--max-city-networks-to-monitor")]
    public int? MaxCityNetworksToMonitor { get; set; }

    [CliOption("--internet-measurements-log-delivery")]
    public string? InternetMeasurementsLogDelivery { get; set; }

    [CliOption("--traffic-percentage-to-monitor")]
    public int? TrafficPercentageToMonitor { get; set; }

    [CliOption("--health-events-config")]
    public string? HealthEventsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}