using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "acknowledge-job")]
public record AwsCodepipelineAcknowledgeJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--nonce")] string Nonce
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}