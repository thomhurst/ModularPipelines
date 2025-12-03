using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "describe-account-assignment-deletion-status")]
public record AwsSsoAdminDescribeAccountAssignmentDeletionStatusOptions(
[property: CliOption("--account-assignment-deletion-request-id")] string AccountAssignmentDeletionRequestId,
[property: CliOption("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}