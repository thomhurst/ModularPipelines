using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "put-job-success-result")]
public record AwsCodepipelinePutJobSuccessResultOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AwsOptions
{
    [CommandSwitch("--current-revision")]
    public string? CurrentRevision { get; set; }

    [CommandSwitch("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CommandSwitch("--execution-details")]
    public string? ExecutionDetails { get; set; }

    [CommandSwitch("--output-variables")]
    public IEnumerable<KeyValue>? OutputVariables { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}