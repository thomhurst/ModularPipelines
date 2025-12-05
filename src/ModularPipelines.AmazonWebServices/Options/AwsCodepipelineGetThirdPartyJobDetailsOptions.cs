using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "get-third-party-job-details")]
public record AwsCodepipelineGetThirdPartyJobDetailsOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}