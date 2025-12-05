using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "register-organization-admin-account")]
public record AwsAuditmanagerRegisterOrganizationAdminAccountOptions(
[property: CliOption("--admin-account-id")] string AdminAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}