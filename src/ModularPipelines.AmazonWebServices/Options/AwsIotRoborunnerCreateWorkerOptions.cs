using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-roborunner", "create-worker")]
public record AwsIotRoborunnerCreateWorkerOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--fleet")] string Fleet
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--additional-transient-properties")]
    public string? AdditionalTransientProperties { get; set; }

    [CliOption("--additional-fixed-properties")]
    public string? AdditionalFixedProperties { get; set; }

    [CliOption("--vendor-properties")]
    public string? VendorProperties { get; set; }

    [CliOption("--position")]
    public string? Position { get; set; }

    [CliOption("--orientation")]
    public string? Orientation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}