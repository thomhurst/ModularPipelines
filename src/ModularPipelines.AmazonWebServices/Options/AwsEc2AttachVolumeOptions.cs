using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "attach-volume")]
public record AwsEc2AttachVolumeOptions(
[property: CliOption("--device")] string Device,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--volume-id")] string VolumeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}