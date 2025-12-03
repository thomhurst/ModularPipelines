using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "put-job-success-result")]
public record AwsCodepipelinePutJobSuccessResultOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--current-revision")]
    public string? CurrentRevision { get; set; }

    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliOption("--execution-details")]
    public string? ExecutionDetails { get; set; }

    [CliOption("--output-variables")]
    public IEnumerable<KeyValue>? OutputVariables { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}