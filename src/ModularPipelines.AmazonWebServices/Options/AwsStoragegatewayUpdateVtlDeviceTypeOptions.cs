using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-vtl-device-type")]
public record AwsStoragegatewayUpdateVtlDeviceTypeOptions(
[property: CommandSwitch("--vtl-device-arn")] string VtlDeviceArn,
[property: CommandSwitch("--device-type")] string DeviceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}