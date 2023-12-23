using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "remove-flow-output")]
public record AwsMediaconnectRemoveFlowOutputOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--output-arn")] string OutputArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}