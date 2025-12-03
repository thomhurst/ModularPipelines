using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "batch-disassociate-client-device-from-core-device")]
public record AwsGreengrassv2BatchDisassociateClientDeviceFromCoreDeviceOptions(
[property: CliOption("--core-device-thing-name")] string CoreDeviceThingName
) : AwsOptions
{
    [CliOption("--entries")]
    public string[]? Entries { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}