using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "batch-get-device-position")]
public record AwsLocationBatchGetDevicePositionOptions(
[property: CliOption("--device-ids")] string[] DeviceIds,
[property: CliOption("--tracker-name")] string TrackerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}