using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-lens-review")]
public record AwsWellarchitectedUpdateLensReviewOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--lens-notes")]
    public string? LensNotes { get; set; }

    [CliOption("--pillar-notes")]
    public IEnumerable<KeyValue>? PillarNotes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}