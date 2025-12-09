using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-data-quality-ruleset-evaluation-run")]
public record AwsGlueGetDataQualityRulesetEvaluationRunOptions(
[property: CliOption("--run-id")] string RunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}