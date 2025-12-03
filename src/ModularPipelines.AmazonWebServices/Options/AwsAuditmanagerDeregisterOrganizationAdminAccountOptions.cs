using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "deregister-organization-admin-account")]
public record AwsAuditmanagerDeregisterOrganizationAdminAccountOptions : AwsOptions
{
    [CliOption("--admin-account-id")]
    public string? AdminAccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}