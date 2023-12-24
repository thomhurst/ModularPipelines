using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privatenetworks", "get-device-identifier")]
public record AwsPrivatenetworksGetDeviceIdentifierOptions(
[property: CommandSwitch("--device-identifier-arn")] string DeviceIdentifierArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}