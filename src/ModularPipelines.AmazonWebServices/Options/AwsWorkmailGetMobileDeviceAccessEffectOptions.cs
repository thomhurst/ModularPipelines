using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "get-mobile-device-access-effect")]
public record AwsWorkmailGetMobileDeviceAccessEffectOptions(
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--device-type")]
    public string? DeviceType { get; set; }

    [CliOption("--device-model")]
    public string? DeviceModel { get; set; }

    [CliOption("--device-operating-system")]
    public string? DeviceOperatingSystem { get; set; }

    [CliOption("--device-user-agent")]
    public string? DeviceUserAgent { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}