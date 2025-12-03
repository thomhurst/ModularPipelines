using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "register-organization-delegated-admin")]
public record AwsCloudtrailRegisterOrganizationDelegatedAdminOptions(
[property: CliOption("--member-account-id")] string MemberAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}