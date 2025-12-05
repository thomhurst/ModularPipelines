using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internetmonitor", "create-monitor")]
public record AwsInternetmonitorCreateMonitorOptions(
[property: CliOption("--monitor-name")] string MonitorName
) : AwsOptions
{
    [CliOption("--resources")]
    public string[]? Resources { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

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