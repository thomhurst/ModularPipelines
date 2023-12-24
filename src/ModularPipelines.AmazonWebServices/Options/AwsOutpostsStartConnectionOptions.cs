using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "start-connection")]
public record AwsOutpostsStartConnectionOptions(
[property: CommandSwitch("--device-serial-number")] string DeviceSerialNumber,
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--client-public-key")] string ClientPublicKey,
[property: CommandSwitch("--network-interface-device-index")] int NetworkInterfaceDeviceIndex
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}