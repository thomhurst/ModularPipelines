using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-batch-segment-job")]
public record AwsPersonalizeCreateBatchSegmentJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--solution-version-arn")] string SolutionVersionArn,
[property: CliOption("--job-input")] string JobInput,
[property: CliOption("--job-output")] string JobOutput,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--filter-arn")]
    public string? FilterArn { get; set; }

    [CliOption("--num-results")]
    public int? NumResults { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}