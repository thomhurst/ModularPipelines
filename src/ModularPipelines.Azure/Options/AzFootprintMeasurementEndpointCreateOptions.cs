using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("footprint", "measurement-endpoint", "create")]
public record AzFootprintMeasurementEndpointCreateOptions(
[property: CliOption("--endpoint")] string Endpoint,
[property: CliOption("--measurement-endpoint-name")] string MeasurementEndpointName,
[property: CliOption("--measurement-type")] string MeasurementType,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--weight")] string Weight
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--end-time-utc")]
    public string? EndTimeUtc { get; set; }

    [CliOption("--experiment-id")]
    public string? ExperimentId { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--object-path")]
    public string? ObjectPath { get; set; }

    [CliOption("--sample-rate-cold")]
    public string? SampleRateCold { get; set; }

    [CliOption("--sample-rate-hot")]
    public string? SampleRateHot { get; set; }

    [CliOption("--sample-rate-warm")]
    public string? SampleRateWarm { get; set; }

    [CliOption("--start-time-utc")]
    public string? StartTimeUtc { get; set; }
}