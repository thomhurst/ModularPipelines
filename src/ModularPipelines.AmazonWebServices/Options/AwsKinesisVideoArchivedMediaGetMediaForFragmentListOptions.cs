using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis-video-archived-media", "get-media-for-fragment-list")]
public record AwsKinesisVideoArchivedMediaGetMediaForFragmentListOptions(
[property: CliOption("--fragments")] string[] Fragments
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }
}