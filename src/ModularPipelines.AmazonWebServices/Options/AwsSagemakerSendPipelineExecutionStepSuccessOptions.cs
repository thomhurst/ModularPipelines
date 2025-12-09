using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "send-pipeline-execution-step-success")]
public record AwsSagemakerSendPipelineExecutionStepSuccessOptions(
[property: CliOption("--callback-token")] string CallbackToken
) : AwsOptions
{
    [CliOption("--output-parameters")]
    public string[]? OutputParameters { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}