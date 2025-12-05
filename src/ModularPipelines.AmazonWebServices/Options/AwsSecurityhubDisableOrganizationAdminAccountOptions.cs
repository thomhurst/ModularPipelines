using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "disable-organization-admin-account")]
public record AwsSecurityhubDisableOrganizationAdminAccountOptions(
[property: CliOption("--admin-account-id")] string AdminAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}