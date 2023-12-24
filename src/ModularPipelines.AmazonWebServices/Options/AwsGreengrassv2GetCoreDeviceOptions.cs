using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "get-core-device")]
public record AwsGreengrassv2GetCoreDeviceOptions(
[property: CommandSwitch("--core-device-thing-name")] string CoreDeviceThingName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}