using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "deregister-organization-admin-account")]
public record AwsAuditmanagerDeregisterOrganizationAdminAccountOptions : AwsOptions
{
    [CommandSwitch("--admin-account-id")]
    public string? AdminAccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}