using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "poll-for-jobs")]
public record AwsCodepipelinePollForJobsOptions(
[property: CommandSwitch("--action-type-id")] string ActionTypeId
) : AwsOptions
{
    [CommandSwitch("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CommandSwitch("--query-param")]
    public IEnumerable<KeyValue>? QueryParam { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}