using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-iam-policy-assignment")]
public record AwsQuicksightDeleteIamPolicyAssignmentOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--assignment-name")] string AssignmentName,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}