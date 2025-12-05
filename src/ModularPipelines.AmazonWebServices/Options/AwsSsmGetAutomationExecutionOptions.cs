using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-automation-execution")]
public record AwsSsmGetAutomationExecutionOptions(
[property: CliOption("--automation-execution-id")] string AutomationExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}