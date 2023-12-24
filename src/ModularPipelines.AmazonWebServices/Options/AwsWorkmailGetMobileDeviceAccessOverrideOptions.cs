using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "get-mobile-device-access-override")]
public record AwsWorkmailGetMobileDeviceAccessOverrideOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}