using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("footprint", "profile", "create")]
public record AzFootprintProfileCreateOptions(
[property: CliOption("--measurement-count")] int MeasurementCount,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--start-delay-ms")] string StartDelayMs
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--reporting-endpoints")]
    public string? ReportingEndpoints { get; set; }

    [CliOption("--sample-rate-cold")]
    public string? SampleRateCold { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}