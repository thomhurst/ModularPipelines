using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-volume")]
public record AwsEc2ModifyVolumeOptions(
[property: CliOption("--volume-id")] string VolumeId
) : AwsOptions
{
    [CliOption("--size")]
    public int? Size { get; set; }

    [CliOption("--volume-type")]
    public string? VolumeType { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--throughput")]
    public int? Throughput { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}