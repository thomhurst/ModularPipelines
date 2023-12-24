using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-device-definition-version")]
public record AwsGreengrassGetDeviceDefinitionVersionOptions(
[property: CommandSwitch("--device-definition-id")] string DeviceDefinitionId,
[property: CommandSwitch("--device-definition-version-id")] string DeviceDefinitionVersionId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}