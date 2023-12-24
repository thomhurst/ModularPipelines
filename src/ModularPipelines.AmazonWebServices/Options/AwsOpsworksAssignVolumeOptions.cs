using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "assign-volume")]
public record AwsOpsworksAssignVolumeOptions(
[property: CommandSwitch("--volume-id")] string VolumeId
) : AwsOptions
{
    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}