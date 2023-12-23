using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-data-quality-rule-recommendation-run")]
public record AwsGlueStartDataQualityRuleRecommendationRunOptions(
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--role")] string Role
) : AwsOptions
{
    [CommandSwitch("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--created-ruleset-name")]
    public string? CreatedRulesetName { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}