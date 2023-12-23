using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-external-evaluation")]
public record AwsConfigservicePutExternalEvaluationOptions(
[property: CommandSwitch("--config-rule-name")] string ConfigRuleName,
[property: CommandSwitch("--external-evaluation")] string ExternalEvaluation
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}