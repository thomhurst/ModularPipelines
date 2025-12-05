using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "poll-for-jobs")]
public record AwsCodepipelinePollForJobsOptions(
[property: CliOption("--action-type-id")] string ActionTypeId
) : AwsOptions
{
    [CliOption("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CliOption("--query-param")]
    public IEnumerable<KeyValue>? QueryParam { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}