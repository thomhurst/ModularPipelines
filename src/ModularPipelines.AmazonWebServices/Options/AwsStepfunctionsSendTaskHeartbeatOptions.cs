using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "send-task-heartbeat")]
public record AwsStepfunctionsSendTaskHeartbeatOptions(
[property: CliOption("--task-token")] string TaskToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}