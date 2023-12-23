using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "attach-disk")]
public record AwsLightsailAttachDiskOptions(
[property: CommandSwitch("--disk-name")] string DiskName,
[property: CommandSwitch("--instance-name")] string InstanceName,
[property: CommandSwitch("--disk-path")] string DiskPath
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}