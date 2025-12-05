using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "create-job-for-devices")]
public record AwsPanoramaCreateJobForDevicesOptions(
[property: CliOption("--device-ids")] string[] DeviceIds,
[property: CliOption("--job-type")] string JobType
) : AwsOptions
{
    [CliOption("--device-job-config")]
    public string? DeviceJobConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}