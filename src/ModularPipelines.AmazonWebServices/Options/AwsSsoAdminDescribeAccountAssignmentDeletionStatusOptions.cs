using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "describe-account-assignment-deletion-status")]
public record AwsSsoAdminDescribeAccountAssignmentDeletionStatusOptions(
[property: CommandSwitch("--account-assignment-deletion-request-id")] string AccountAssignmentDeletionRequestId,
[property: CommandSwitch("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}