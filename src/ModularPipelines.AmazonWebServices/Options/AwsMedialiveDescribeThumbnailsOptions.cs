using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "describe-thumbnails")]
public record AwsMedialiveDescribeThumbnailsOptions(
[property: CommandSwitch("--channel-id")] string ChannelId,
[property: CommandSwitch("--pipeline-id")] string PipelineId,
[property: CommandSwitch("--thumbnail-type")] string ThumbnailType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}