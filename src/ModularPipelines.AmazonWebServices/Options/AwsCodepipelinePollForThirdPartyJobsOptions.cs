using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "poll-for-third-party-jobs")]
public record AwsCodepipelinePollForThirdPartyJobsOptions(
[property: CliOption("--action-type-id")] string ActionTypeId
) : AwsOptions
{
    [CliOption("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}