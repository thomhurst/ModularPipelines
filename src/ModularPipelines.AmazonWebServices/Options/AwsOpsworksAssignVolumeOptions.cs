using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "assign-volume")]
public record AwsOpsworksAssignVolumeOptions(
[property: CliOption("--volume-id")] string VolumeId
) : AwsOptions
{
    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}