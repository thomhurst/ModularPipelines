using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "start-sync-execution")]
public record AwsStepfunctionsStartSyncExecutionOptions(
[property: CommandSwitch("--state-machine-arn")] string StateMachineArn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [CommandSwitch("--trace-header")]
    public string? TraceHeader { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}