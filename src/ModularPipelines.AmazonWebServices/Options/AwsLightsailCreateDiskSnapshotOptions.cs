using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-disk-snapshot")]
public record AwsLightsailCreateDiskSnapshotOptions(
[property: CommandSwitch("--disk-snapshot-name")] string DiskSnapshotName
) : AwsOptions
{
    [CommandSwitch("--disk-name")]
    public string? DiskName { get; set; }

    [CommandSwitch("--instance-name")]
    public string? InstanceName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}