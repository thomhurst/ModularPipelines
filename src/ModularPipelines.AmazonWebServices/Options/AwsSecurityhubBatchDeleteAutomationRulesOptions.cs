using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-delete-automation-rules")]
public record AwsSecurityhubBatchDeleteAutomationRulesOptions(
[property: CliOption("--automation-rules-arns")] string[] AutomationRulesArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}