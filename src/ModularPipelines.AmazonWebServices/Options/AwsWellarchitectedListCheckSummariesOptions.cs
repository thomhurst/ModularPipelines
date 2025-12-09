using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "list-check-summaries")]
public record AwsWellarchitectedListCheckSummariesOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-arn")] string LensArn,
[property: CliOption("--pillar-id")] string PillarId,
[property: CliOption("--question-id")] string QuestionId,
[property: CliOption("--choice-id")] string ChoiceId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}