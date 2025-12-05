using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "add-flow-media-streams")]
public record AwsMediaconnectAddFlowMediaStreamsOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--media-streams")] string[] MediaStreams
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}