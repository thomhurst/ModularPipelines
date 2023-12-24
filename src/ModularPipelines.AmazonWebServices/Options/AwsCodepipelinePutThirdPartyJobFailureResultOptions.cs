using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "put-third-party-job-failure-result")]
public record AwsCodepipelinePutThirdPartyJobFailureResultOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--failure-details")] string FailureDetails
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}