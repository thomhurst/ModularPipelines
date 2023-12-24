using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-archived-media", "get-media-for-fragment-list")]
public record AwsKinesisVideoArchivedMediaGetMediaForFragmentListOptions(
[property: CommandSwitch("--fragments")] string[] Fragments
) : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }
}