using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-answer")]
public record AwsWellarchitectedUpdateAnswerOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-alias")] string LensAlias,
[property: CliOption("--question-id")] string QuestionId
) : AwsOptions
{
    [CliOption("--selected-choices")]
    public string[]? SelectedChoices { get; set; }

    [CliOption("--choice-updates")]
    public IEnumerable<KeyValue>? ChoiceUpdates { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}