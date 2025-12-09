using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "evaluate-feature")]
public record AwsEvidentlyEvaluateFeatureOptions(
[property: CliOption("--entity-id")] string EntityId,
[property: CliOption("--feature")] string Feature,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--evaluation-context")]
    public string? EvaluationContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}