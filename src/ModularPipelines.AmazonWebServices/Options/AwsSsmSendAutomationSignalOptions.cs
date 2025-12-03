using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "send-automation-signal")]
public record AwsSsmSendAutomationSignalOptions(
[property: CliOption("--automation-execution-id")] string AutomationExecutionId,
[property: CliOption("--signal-type")] string SignalType
) : AwsOptions
{
    [CliOption("--payload")]
    public IEnumerable<KeyValue>? Payload { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}