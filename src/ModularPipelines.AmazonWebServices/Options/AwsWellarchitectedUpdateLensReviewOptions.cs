using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-lens-review")]
public record AwsWellarchitectedUpdateLensReviewOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CommandSwitch("--lens-notes")]
    public string? LensNotes { get; set; }

    [CommandSwitch("--pillar-notes")]
    public IEnumerable<KeyValue>? PillarNotes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}