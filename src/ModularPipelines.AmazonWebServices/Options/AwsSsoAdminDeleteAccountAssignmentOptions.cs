using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "delete-account-assignment")]
public record AwsSsoAdminDeleteAccountAssignmentOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--principal-type")] string PrincipalType,
[property: CliOption("--target-id")] string TargetId,
[property: CliOption("--target-type")] string TargetType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}