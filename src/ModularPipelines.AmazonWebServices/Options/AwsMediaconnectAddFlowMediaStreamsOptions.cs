using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "add-flow-media-streams")]
public record AwsMediaconnectAddFlowMediaStreamsOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--media-streams")] string[] MediaStreams
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}