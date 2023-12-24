using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-update-automation-rules")]
public record AwsSecurityhubBatchUpdateAutomationRulesOptions(
[property: CommandSwitch("--update-automation-rules-request-items")] string[] UpdateAutomationRulesRequestItems
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}