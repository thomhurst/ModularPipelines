using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "list-check-summaries")]
public record AwsWellarchitectedListCheckSummariesOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--lens-arn")] string LensArn,
[property: CommandSwitch("--pillar-id")] string PillarId,
[property: CommandSwitch("--question-id")] string QuestionId,
[property: CommandSwitch("--choice-id")] string ChoiceId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}