using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "update-job-priority")]
public record AwsS3controlUpdateJobPriorityOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--priority")] int Priority
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}