using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-volumes")]
public record AwsOpsworksDescribeVolumesOptions : AwsOptions
{
    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--raid-array-id")]
    public string? RaidArrayId { get; set; }

    [CliOption("--volume-ids")]
    public string[]? VolumeIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}