using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("footprint", "measurement-endpoint", "update")]
public record AzFootprintMeasurementEndpointUpdateOptions(
[property: CommandSwitch("--endpoint")] string Endpoint,
[property: CommandSwitch("--measurement-type")] string MeasurementType,
[property: CommandSwitch("--weight")] string Weight
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--end-time-utc")]
    public string? EndTimeUtc { get; set; }

    [CommandSwitch("--experiment-id")]
    public string? ExperimentId { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--measurement-endpoint-name")]
    public string? MeasurementEndpointName { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--object-path")]
    public string? ObjectPath { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sample-rate-cold")]
    public string? SampleRateCold { get; set; }

    [CommandSwitch("--sample-rate-hot")]
    public string? SampleRateHot { get; set; }

    [CommandSwitch("--sample-rate-warm")]
    public string? SampleRateWarm { get; set; }

    [CommandSwitch("--start-time-utc")]
    public string? StartTimeUtc { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}