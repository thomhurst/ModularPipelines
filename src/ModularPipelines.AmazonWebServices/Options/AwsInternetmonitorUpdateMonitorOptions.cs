using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internetmonitor", "update-monitor")]
public record AwsInternetmonitorUpdateMonitorOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName
) : AwsOptions
{
    [CommandSwitch("--resources-to-add")]
    public string[]? ResourcesToAdd { get; set; }

    [CommandSwitch("--resources-to-remove")]
    public string[]? ResourcesToRemove { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

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