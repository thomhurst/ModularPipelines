using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-instance-snapshot")]
public record AwsLightsailDeleteInstanceSnapshotOptions(
[property: CommandSwitch("--instance-snapshot-name")] string InstanceSnapshotName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}