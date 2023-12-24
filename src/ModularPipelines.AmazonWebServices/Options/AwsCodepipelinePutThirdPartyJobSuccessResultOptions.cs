using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "put-third-party-job-success-result")]
public record AwsCodepipelinePutThirdPartyJobSuccessResultOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--current-revision")]
    public string? CurrentRevision { get; set; }

    [CommandSwitch("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CommandSwitch("--execution-details")]
    public string? ExecutionDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}