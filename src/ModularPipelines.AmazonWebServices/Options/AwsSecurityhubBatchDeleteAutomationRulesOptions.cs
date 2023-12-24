using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-delete-automation-rules")]
public record AwsSecurityhubBatchDeleteAutomationRulesOptions(
[property: CommandSwitch("--automation-rules-arns")] string[] AutomationRulesArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}