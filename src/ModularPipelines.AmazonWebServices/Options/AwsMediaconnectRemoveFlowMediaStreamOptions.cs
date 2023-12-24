using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "remove-flow-media-stream")]
public record AwsMediaconnectRemoveFlowMediaStreamOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--media-stream-name")] string MediaStreamName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}