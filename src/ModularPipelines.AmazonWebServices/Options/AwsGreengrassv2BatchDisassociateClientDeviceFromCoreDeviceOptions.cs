using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "batch-disassociate-client-device-from-core-device")]
public record AwsGreengrassv2BatchDisassociateClientDeviceFromCoreDeviceOptions(
[property: CommandSwitch("--core-device-thing-name")] string CoreDeviceThingName
) : AwsOptions
{
    [CommandSwitch("--entries")]
    public string[]? Entries { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}