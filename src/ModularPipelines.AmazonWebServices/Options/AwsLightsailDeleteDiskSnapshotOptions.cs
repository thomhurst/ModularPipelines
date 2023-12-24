using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-disk-snapshot")]
public record AwsLightsailDeleteDiskSnapshotOptions(
[property: CommandSwitch("--disk-snapshot-name")] string DiskSnapshotName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}