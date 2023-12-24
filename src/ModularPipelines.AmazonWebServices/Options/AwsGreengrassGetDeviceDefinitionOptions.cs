using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-device-definition")]
public record AwsGreengrassGetDeviceDefinitionOptions(
[property: CommandSwitch("--device-definition-id")] string DeviceDefinitionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}