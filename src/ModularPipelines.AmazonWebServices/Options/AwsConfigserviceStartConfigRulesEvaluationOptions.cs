using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "start-config-rules-evaluation")]
public record AwsConfigserviceStartConfigRulesEvaluationOptions : AwsOptions
{
    [CommandSwitch("--config-rule-names")]
    public string[]? ConfigRuleNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}