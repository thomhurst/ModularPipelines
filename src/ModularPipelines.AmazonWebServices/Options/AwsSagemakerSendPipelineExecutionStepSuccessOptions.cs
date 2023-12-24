using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "send-pipeline-execution-step-success")]
public record AwsSagemakerSendPipelineExecutionStepSuccessOptions(
[property: CommandSwitch("--callback-token")] string CallbackToken
) : AwsOptions
{
    [CommandSwitch("--output-parameters")]
    public string[]? OutputParameters { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}