using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "create-device-definition-version")]
public record AwsGreengrassCreateDeviceDefinitionVersionOptions(
[property: CommandSwitch("--device-definition-id")] string DeviceDefinitionId
) : AwsOptions
{
    [CommandSwitch("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CommandSwitch("--devices")]
    public string[]? Devices { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}