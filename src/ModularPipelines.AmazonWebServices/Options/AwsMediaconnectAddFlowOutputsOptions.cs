using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "add-flow-outputs")]
public record AwsMediaconnectAddFlowOutputsOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--outputs")] string[] Outputs
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}