using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-iam-policy-assignment")]
public record AwsQuicksightUpdateIamPolicyAssignmentOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--assignment-name")] string AssignmentName,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--assignment-status")]
    public string? AssignmentStatus { get; set; }

    [CliOption("--policy-arn")]
    public string? PolicyArn { get; set; }

    [CliOption("--identities")]
    public IEnumerable<KeyValue>? Identities { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}