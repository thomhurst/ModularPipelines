using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "detach-managed-policy-from-permission-set")]
public record AwsSsoAdminDetachManagedPolicyFromPermissionSetOptions(
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--managed-policy-arn")] string ManagedPolicyArn,
[property: CommandSwitch("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}