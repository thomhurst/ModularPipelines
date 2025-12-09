using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "acknowledge-third-party-job")]
public record AwsCodepipelineAcknowledgeThirdPartyJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--nonce")] string Nonce,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}