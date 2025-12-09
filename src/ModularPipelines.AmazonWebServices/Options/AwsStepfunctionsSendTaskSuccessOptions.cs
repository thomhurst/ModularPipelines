using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "send-task-success")]
public record AwsStepfunctionsSendTaskSuccessOptions(
[property: CliOption("--task-token")] string TaskToken,
[property: CliOption("--task-output")] string TaskOutput
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}