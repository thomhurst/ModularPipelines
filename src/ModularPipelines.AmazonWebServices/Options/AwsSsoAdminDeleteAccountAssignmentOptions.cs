using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "delete-account-assignment")]
public record AwsSsoAdminDeleteAccountAssignmentOptions(
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--permission-set-arn")] string PermissionSetArn,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--principal-type")] string PrincipalType,
[property: CommandSwitch("--target-id")] string TargetId,
[property: CommandSwitch("--target-type")] string TargetType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}