using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internetmonitor", "create-monitor")]
public record AwsInternetmonitorCreateMonitorOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName
) : AwsOptions
{
    [CommandSwitch("--resources")]
    public string[]? Resources { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--max-city-networks-to-monitor")]
    public int? MaxCityNetworksToMonitor { get; set; }

    [CommandSwitch("--internet-measurements-log-delivery")]
    public string? InternetMeasurementsLogDelivery { get; set; }

    [CommandSwitch("--traffic-percentage-to-monitor")]
    public int? TrafficPercentageToMonitor { get; set; }

    [CommandSwitch("--health-events-config")]
    public string? HealthEventsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}