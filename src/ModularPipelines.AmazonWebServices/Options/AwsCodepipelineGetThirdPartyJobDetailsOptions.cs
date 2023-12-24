using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "get-third-party-job-details")]
public record AwsCodepipelineGetThirdPartyJobDetailsOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}