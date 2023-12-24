using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "acknowledge-third-party-job")]
public record AwsCodepipelineAcknowledgeThirdPartyJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--nonce")] string Nonce,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}