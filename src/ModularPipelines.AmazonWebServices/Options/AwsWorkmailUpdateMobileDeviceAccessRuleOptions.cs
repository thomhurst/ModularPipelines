using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "update-mobile-device-access-rule")]
public record AwsWorkmailUpdateMobileDeviceAccessRuleOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--mobile-device-access-rule-id")] string MobileDeviceAccessRuleId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--effect")] string Effect
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--device-types")]
    public string[]? DeviceTypes { get; set; }

    [CommandSwitch("--not-device-types")]
    public string[]? NotDeviceTypes { get; set; }

    [CommandSwitch("--device-models")]
    public string[]? DeviceModels { get; set; }

    [CommandSwitch("--not-device-models")]
    public string[]? NotDeviceModels { get; set; }

    [CommandSwitch("--device-operating-systems")]
    public string[]? DeviceOperatingSystems { get; set; }

    [CommandSwitch("--not-device-operating-systems")]
    public string[]? NotDeviceOperatingSystems { get; set; }

    [CommandSwitch("--device-user-agents")]
    public string[]? DeviceUserAgents { get; set; }

    [CommandSwitch("--not-device-user-agents")]
    public string[]? NotDeviceUserAgents { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}