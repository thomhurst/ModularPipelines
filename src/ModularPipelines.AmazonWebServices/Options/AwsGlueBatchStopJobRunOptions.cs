using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "batch-stop-job-run")]
public record AwsGlueBatchStopJobRunOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--job-run-ids")] string[] JobRunIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}