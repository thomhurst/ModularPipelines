using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "attach-customer-managed-policy-reference-to-permission-set")]
public record AwsSsoAdminAttachCustomerManagedPolicyReferenceToPermissionSetOptions(
[property: CliOption("--customer-managed-policy-reference")] string CustomerManagedPolicyReference,
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}