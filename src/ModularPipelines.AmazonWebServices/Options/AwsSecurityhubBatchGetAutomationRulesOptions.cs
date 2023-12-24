using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-get-automation-rules")]
public record AwsSecurityhubBatchGetAutomationRulesOptions(
[property: CommandSwitch("--automation-rules-arns")] string[] AutomationRulesArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}