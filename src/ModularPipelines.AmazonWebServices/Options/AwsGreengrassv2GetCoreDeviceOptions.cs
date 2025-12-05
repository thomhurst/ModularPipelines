using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "get-core-device")]
public record AwsGreengrassv2GetCoreDeviceOptions(
[property: CliOption("--core-device-thing-name")] string CoreDeviceThingName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}