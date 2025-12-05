using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "put-job-failure-result")]
public record AwsCodepipelinePutJobFailureResultOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--failure-details")] string FailureDetails
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}