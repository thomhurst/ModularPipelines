using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "create-mobile-device-access-rule")]
public record AwsWorkmailCreateMobileDeviceAccessRuleOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--name")] string Name,
[property: CliOption("--effect")] string Effect
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--device-types")]
    public string[]? DeviceTypes { get; set; }

    [CliOption("--not-device-types")]
    public string[]? NotDeviceTypes { get; set; }

    [CliOption("--device-models")]
    public string[]? DeviceModels { get; set; }

    [CliOption("--not-device-models")]
    public string[]? NotDeviceModels { get; set; }

    [CliOption("--device-operating-systems")]
    public string[]? DeviceOperatingSystems { get; set; }

    [CliOption("--not-device-operating-systems")]
    public string[]? NotDeviceOperatingSystems { get; set; }

    [CliOption("--device-user-agents")]
    public string[]? DeviceUserAgents { get; set; }

    [CliOption("--not-device-user-agents")]
    public string[]? NotDeviceUserAgents { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}