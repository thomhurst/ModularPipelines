using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-volume")]
public record AwsEc2ModifyVolumeOptions(
[property: CommandSwitch("--volume-id")] string VolumeId
) : AwsOptions
{
    [CommandSwitch("--size")]
    public int? Size { get; set; }

    [CommandSwitch("--volume-type")]
    public string? VolumeType { get; set; }

    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--throughput")]
    public int? Throughput { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}