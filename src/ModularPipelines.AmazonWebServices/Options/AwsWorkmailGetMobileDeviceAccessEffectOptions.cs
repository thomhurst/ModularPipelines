using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "get-mobile-device-access-effect")]
public record AwsWorkmailGetMobileDeviceAccessEffectOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CommandSwitch("--device-type")]
    public string? DeviceType { get; set; }

    [CommandSwitch("--device-model")]
    public string? DeviceModel { get; set; }

    [CommandSwitch("--device-operating-system")]
    public string? DeviceOperatingSystem { get; set; }

    [CommandSwitch("--device-user-agent")]
    public string? DeviceUserAgent { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}