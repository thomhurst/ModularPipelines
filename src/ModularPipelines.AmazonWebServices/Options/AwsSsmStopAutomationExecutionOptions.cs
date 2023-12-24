using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "stop-automation-execution")]
public record AwsSsmStopAutomationExecutionOptions(
[property: CommandSwitch("--automation-execution-id")] string AutomationExecutionId
) : AwsOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}