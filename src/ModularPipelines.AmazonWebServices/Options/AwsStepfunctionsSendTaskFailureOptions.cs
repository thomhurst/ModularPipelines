using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "send-task-failure")]
public record AwsStepfunctionsSendTaskFailureOptions(
[property: CliOption("--task-token")] string TaskToken
) : AwsOptions
{
    [CliOption("--error")]
    public string? Error { get; set; }

    [CliOption("--cause")]
    public string? Cause { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}