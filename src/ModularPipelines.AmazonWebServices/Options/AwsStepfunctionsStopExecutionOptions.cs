using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "stop-execution")]
public record AwsStepfunctionsStopExecutionOptions(
[property: CliOption("--execution-arn")] string ExecutionArn
) : AwsOptions
{
    [CliOption("--error")]
    public string? Error { get; set; }

    [CliOption("--cause")]
    public string? Cause { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}