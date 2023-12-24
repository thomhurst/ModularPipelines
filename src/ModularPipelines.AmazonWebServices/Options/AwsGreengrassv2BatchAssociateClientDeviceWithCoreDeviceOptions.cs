using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "batch-associate-client-device-with-core-device")]
public record AwsGreengrassv2BatchAssociateClientDeviceWithCoreDeviceOptions(
[property: CommandSwitch("--core-device-thing-name")] string CoreDeviceThingName
) : AwsOptions
{
    [CommandSwitch("--entries")]
    public string[]? Entries { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}