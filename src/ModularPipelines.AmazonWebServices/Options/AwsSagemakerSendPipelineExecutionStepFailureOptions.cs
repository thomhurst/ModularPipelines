using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "send-pipeline-execution-step-failure")]
public record AwsSagemakerSendPipelineExecutionStepFailureOptions(
[property: CliOption("--callback-token")] string CallbackToken
) : AwsOptions
{
    [CliOption("--failure-reason")]
    public string? FailureReason { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}