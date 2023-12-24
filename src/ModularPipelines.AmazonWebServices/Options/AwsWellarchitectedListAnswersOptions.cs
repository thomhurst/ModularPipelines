using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "list-answers")]
public record AwsWellarchitectedListAnswersOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CommandSwitch("--pillar-id")]
    public string? PillarId { get; set; }

    [CommandSwitch("--milestone-number")]
    public int? MilestoneNumber { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--question-priority")]
    public string? QuestionPriority { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}