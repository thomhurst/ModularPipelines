using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "remove-flow-media-stream")]
public record AwsMediaconnectRemoveFlowMediaStreamOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--media-stream-name")] string MediaStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}