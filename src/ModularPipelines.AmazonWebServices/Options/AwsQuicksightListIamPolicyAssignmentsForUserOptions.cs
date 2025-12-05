using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "list-iam-policy-assignments-for-user")]
public record AwsQuicksightListIamPolicyAssignmentsForUserOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}