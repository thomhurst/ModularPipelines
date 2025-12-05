using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-vtl-device-type")]
public record AwsStoragegatewayUpdateVtlDeviceTypeOptions(
[property: CliOption("--vtl-device-arn")] string VtlDeviceArn,
[property: CliOption("--device-type")] string DeviceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}