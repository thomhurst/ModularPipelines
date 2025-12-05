using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "cancel-data-quality-ruleset-evaluation-run")]
public record AwsGlueCancelDataQualityRulesetEvaluationRunOptions(
[property: CliOption("--run-id")] string RunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}