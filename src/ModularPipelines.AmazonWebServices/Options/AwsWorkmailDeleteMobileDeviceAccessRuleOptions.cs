using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "delete-mobile-device-access-rule")]
public record AwsWorkmailDeleteMobileDeviceAccessRuleOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--mobile-device-access-rule-id")] string MobileDeviceAccessRuleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}