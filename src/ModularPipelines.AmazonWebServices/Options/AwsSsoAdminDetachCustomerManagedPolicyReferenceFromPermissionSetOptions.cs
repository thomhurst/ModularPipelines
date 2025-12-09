using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "detach-customer-managed-policy-reference-from-permission-set")]
public record AwsSsoAdminDetachCustomerManagedPolicyReferenceFromPermissionSetOptions(
[property: CliOption("--customer-managed-policy-reference")] string CustomerManagedPolicyReference,
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}