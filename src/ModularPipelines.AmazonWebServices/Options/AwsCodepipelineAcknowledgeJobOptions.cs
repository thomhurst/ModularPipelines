using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "acknowledge-job")]
public record AwsCodepipelineAcknowledgeJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--nonce")] string Nonce
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}