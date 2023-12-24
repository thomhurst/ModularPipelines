using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "deregister-organization-delegated-admin")]
public record AwsCloudtrailDeregisterOrganizationDelegatedAdminOptions(
[property: CommandSwitch("--delegated-admin-account-id")] string DelegatedAdminAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}