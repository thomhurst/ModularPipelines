using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "send-task-heartbeat")]
public record AwsStepfunctionsSendTaskHeartbeatOptions(
[property: CommandSwitch("--task-token")] string TaskToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}