using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-answer")]
public record AwsWellarchitectedGetAnswerOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-alias")] string LensAlias,
[property: CliOption("--question-id")] string QuestionId
) : AwsOptions
{
    [CliOption("--milestone-number")]
    public int? MilestoneNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}