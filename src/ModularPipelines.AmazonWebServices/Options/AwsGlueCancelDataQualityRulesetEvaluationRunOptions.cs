using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "cancel-data-quality-ruleset-evaluation-run")]
public record AwsGlueCancelDataQualityRulesetEvaluationRunOptions(
[property: CommandSwitch("--run-id")] string RunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}