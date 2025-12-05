using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "put-third-party-job-success-result")]
public record AwsCodepipelinePutThirdPartyJobSuccessResultOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--current-revision")]
    public string? CurrentRevision { get; set; }

    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliOption("--execution-details")]
    public string? ExecutionDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}