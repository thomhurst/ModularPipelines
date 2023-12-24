using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-a2i-runtime", "start-human-loop")]
public record AwsSagemakerA2iRuntimeStartHumanLoopOptions(
[property: CommandSwitch("--human-loop-name")] string HumanLoopName,
[property: CommandSwitch("--flow-definition-arn")] string FlowDefinitionArn,
[property: CommandSwitch("--human-loop-input")] string HumanLoopInput
) : AwsOptions
{
    [CommandSwitch("--data-attributes")]
    public string? DataAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}