using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "attach-customer-managed-policy-reference-to-permission-set")]
public record AwsSsoAdminAttachCustomerManagedPolicyReferenceToPermissionSetOptions(
[property: CommandSwitch("--customer-managed-policy-reference")] string CustomerManagedPolicyReference,
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}