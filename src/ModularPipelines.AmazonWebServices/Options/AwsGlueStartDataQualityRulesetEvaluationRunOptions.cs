using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-data-quality-ruleset-evaluation-run")]
public record AwsGlueStartDataQualityRulesetEvaluationRunOptions(
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--role")] string Role,
[property: CliOption("--ruleset-names")] string[] RulesetNames
) : AwsOptions
{
    [CliOption("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--additional-run-options")]
    public string? AdditionalRunOptions { get; set; }

    [CliOption("--additional-data-sources")]
    public IEnumerable<KeyValue>? AdditionalDataSources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}