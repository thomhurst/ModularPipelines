using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "put-third-party-job-failure-result")]
public record AwsCodepipelinePutThirdPartyJobFailureResultOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--client-token")] string ClientToken,
[property: CliOption("--failure-details")] string FailureDetails
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}