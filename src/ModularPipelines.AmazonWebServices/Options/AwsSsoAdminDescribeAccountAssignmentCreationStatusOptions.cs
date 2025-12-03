using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "describe-account-assignment-creation-status")]
public record AwsSsoAdminDescribeAccountAssignmentCreationStatusOptions(
[property: CliOption("--account-assignment-creation-request-id")] string AccountAssignmentCreationRequestId,
[property: CliOption("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}