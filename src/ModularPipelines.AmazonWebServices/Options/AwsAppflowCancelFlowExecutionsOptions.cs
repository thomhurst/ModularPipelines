using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "cancel-flow-executions")]
public record AwsAppflowCancelFlowExecutionsOptions(
[property: CommandSwitch("--flow-name")] string FlowName
) : AwsOptions
{
    [CommandSwitch("--execution-ids")]
    public string[]? ExecutionIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}