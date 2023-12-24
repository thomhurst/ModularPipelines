using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "describe-account-assignment-creation-status")]
public record AwsSsoAdminDescribeAccountAssignmentCreationStatusOptions(
[property: CommandSwitch("--account-assignment-creation-request-id")] string AccountAssignmentCreationRequestId,
[property: CommandSwitch("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}