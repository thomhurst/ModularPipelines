using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "stop-automation-execution")]
public record AwsSsmStopAutomationExecutionOptions(
[property: CliOption("--automation-execution-id")] string AutomationExecutionId
) : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}