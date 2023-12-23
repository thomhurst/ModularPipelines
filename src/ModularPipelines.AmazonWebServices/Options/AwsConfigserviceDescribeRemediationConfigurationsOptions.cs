using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "describe-remediation-configurations")]
public record AwsConfigserviceDescribeRemediationConfigurationsOptions(
[property: CommandSwitch("--config-rule-names")] string[] ConfigRuleNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}