using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "batch-associate-client-device-with-core-device")]
public record AwsGreengrassv2BatchAssociateClientDeviceWithCoreDeviceOptions(
[property: CliOption("--core-device-thing-name")] string CoreDeviceThingName
) : AwsOptions
{
    [CliOption("--entries")]
    public string[]? Entries { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}