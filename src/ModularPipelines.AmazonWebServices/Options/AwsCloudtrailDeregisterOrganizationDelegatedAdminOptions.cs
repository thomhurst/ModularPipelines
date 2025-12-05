using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "deregister-organization-delegated-admin")]
public record AwsCloudtrailDeregisterOrganizationDelegatedAdminOptions(
[property: CliOption("--delegated-admin-account-id")] string DelegatedAdminAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}