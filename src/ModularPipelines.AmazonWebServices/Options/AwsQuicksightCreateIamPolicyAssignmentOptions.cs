using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-iam-policy-assignment")]
public record AwsQuicksightCreateIamPolicyAssignmentOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--assignment-name")] string AssignmentName,
[property: CliOption("--assignment-status")] string AssignmentStatus,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--policy-arn")]
    public string? PolicyArn { get; set; }

    [CliOption("--identities")]
    public IEnumerable<KeyValue>? Identities { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}