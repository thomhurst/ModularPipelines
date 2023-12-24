using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "delete-core-device")]
public record AwsGreengrassv2DeleteCoreDeviceOptions(
[property: CommandSwitch("--core-device-thing-name")] string CoreDeviceThingName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}