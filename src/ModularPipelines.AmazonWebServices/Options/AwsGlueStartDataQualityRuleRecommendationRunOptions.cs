using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-data-quality-rule-recommendation-run")]
public record AwsGlueStartDataQualityRuleRecommendationRunOptions(
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--role")] string Role
) : AwsOptions
{
    [CliOption("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--created-ruleset-name")]
    public string? CreatedRulesetName { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}