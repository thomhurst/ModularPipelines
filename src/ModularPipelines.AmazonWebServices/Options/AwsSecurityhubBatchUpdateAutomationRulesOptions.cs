using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-update-automation-rules")]
public record AwsSecurityhubBatchUpdateAutomationRulesOptions(
[property: CliOption("--update-automation-rules-request-items")] string[] UpdateAutomationRulesRequestItems
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}