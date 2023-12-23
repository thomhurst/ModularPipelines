using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-data-quality-ruleset-evaluation-run")]
public record AwsGlueStartDataQualityRulesetEvaluationRunOptions(
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--ruleset-names")] string[] RulesetNames
) : AwsOptions
{
    [CommandSwitch("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--additional-run-options")]
    public string? AdditionalRunOptions { get; set; }

    [CommandSwitch("--additional-data-sources")]
    public IEnumerable<KeyValue>? AdditionalDataSources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}