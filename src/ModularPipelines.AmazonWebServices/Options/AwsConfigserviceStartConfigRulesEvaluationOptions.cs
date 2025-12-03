using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "start-config-rules-evaluation")]
public record AwsConfigserviceStartConfigRulesEvaluationOptions : AwsOptions
{
    [CliOption("--config-rule-names")]
    public string[]? ConfigRuleNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}