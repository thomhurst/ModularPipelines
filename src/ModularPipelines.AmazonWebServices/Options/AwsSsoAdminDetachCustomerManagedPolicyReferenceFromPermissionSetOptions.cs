using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "detach-customer-managed-policy-reference-from-permission-set")]
public record AwsSsoAdminDetachCustomerManagedPolicyReferenceFromPermissionSetOptions(
[property: CommandSwitch("--customer-managed-policy-reference")] string CustomerManagedPolicyReference,
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}