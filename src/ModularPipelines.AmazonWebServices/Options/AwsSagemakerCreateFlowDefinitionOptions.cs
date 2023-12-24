using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-flow-definition")]
public record AwsSagemakerCreateFlowDefinitionOptions(
[property: CommandSwitch("--flow-definition-name")] string FlowDefinitionName,
[property: CommandSwitch("--output-config")] string OutputConfig,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--human-loop-request-source")]
    public string? HumanLoopRequestSource { get; set; }

    [CommandSwitch("--human-loop-activation-config")]
    public string? HumanLoopActivationConfig { get; set; }

    [CommandSwitch("--human-loop-config")]
    public string? HumanLoopConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}