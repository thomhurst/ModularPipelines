using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "delete-mobile-device-access-override")]
public record AwsWorkmailDeleteMobileDeviceAccessOverrideOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}