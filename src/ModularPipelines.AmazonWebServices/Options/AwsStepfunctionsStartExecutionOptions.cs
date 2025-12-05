using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "start-execution")]
public record AwsStepfunctionsStartExecutionOptions(
[property: CliOption("--state-machine-arn")] string StateMachineArn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--input")]
    public string? Input { get; set; }

    [CliOption("--trace-header")]
    public string? TraceHeader { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}