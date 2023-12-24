using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "send-pipeline-execution-step-failure")]
public record AwsSagemakerSendPipelineExecutionStepFailureOptions(
[property: CommandSwitch("--callback-token")] string CallbackToken
) : AwsOptions
{
    [CommandSwitch("--failure-reason")]
    public string? FailureReason { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}