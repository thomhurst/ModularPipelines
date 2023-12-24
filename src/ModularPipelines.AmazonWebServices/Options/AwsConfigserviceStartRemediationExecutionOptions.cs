using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "start-remediation-execution")]
public record AwsConfigserviceStartRemediationExecutionOptions(
[property: CommandSwitch("--config-rule-name")] string ConfigRuleName,
[property: CommandSwitch("--resource-keys")] string[] ResourceKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}