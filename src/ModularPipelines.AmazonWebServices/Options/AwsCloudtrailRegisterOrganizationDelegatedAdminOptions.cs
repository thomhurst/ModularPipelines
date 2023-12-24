using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "register-organization-delegated-admin")]
public record AwsCloudtrailRegisterOrganizationDelegatedAdminOptions(
[property: CommandSwitch("--member-account-id")] string MemberAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}