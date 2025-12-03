using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-device-definition-version")]
public record AwsGreengrassGetDeviceDefinitionVersionOptions(
[property: CliOption("--device-definition-id")] string DeviceDefinitionId,
[property: CliOption("--device-definition-version-id")] string DeviceDefinitionVersionId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}