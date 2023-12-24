using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "disable-organization-admin-account")]
public record AwsGuarddutyDisableOrganizationAdminAccountOptions(
[property: CommandSwitch("--admin-account-id")] string AdminAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}