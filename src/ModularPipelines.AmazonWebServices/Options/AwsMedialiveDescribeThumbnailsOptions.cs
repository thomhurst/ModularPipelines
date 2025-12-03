using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "describe-thumbnails")]
public record AwsMedialiveDescribeThumbnailsOptions(
[property: CliOption("--channel-id")] string ChannelId,
[property: CliOption("--pipeline-id")] string PipelineId,
[property: CliOption("--thumbnail-type")] string ThumbnailType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}