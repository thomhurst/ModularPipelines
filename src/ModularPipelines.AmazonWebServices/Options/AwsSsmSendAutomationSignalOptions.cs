using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "send-automation-signal")]
public record AwsSsmSendAutomationSignalOptions(
[property: CommandSwitch("--automation-execution-id")] string AutomationExecutionId,
[property: CommandSwitch("--signal-type")] string SignalType
) : AwsOptions
{
    [CommandSwitch("--payload")]
    public IEnumerable<KeyValue>? Payload { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}