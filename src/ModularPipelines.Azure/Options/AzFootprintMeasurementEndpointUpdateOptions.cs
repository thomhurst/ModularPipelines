using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("footprint", "measurement-endpoint", "update")]
public record AzFootprintMeasurementEndpointUpdateOptions(
[property: CliOption("--endpoint")] string Endpoint,
[property: CliOption("--measurement-type")] string MeasurementType,
[property: CliOption("--weight")] string Weight
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--end-time-utc")]
    public string? EndTimeUtc { get; set; }

    [CliOption("--experiment-id")]
    public string? ExperimentId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--measurement-endpoint-name")]
    public string? MeasurementEndpointName { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--object-path")]
    public string? ObjectPath { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sample-rate-cold")]
    public string? SampleRateCold { get; set; }

    [CliOption("--sample-rate-hot")]
    public string? SampleRateHot { get; set; }

    [CliOption("--sample-rate-warm")]
    public string? SampleRateWarm { get; set; }

    [CliOption("--start-time-utc")]
    public string? StartTimeUtc { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}