using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-archived-media", "get-clip")]
public record AwsKinesisVideoArchivedMediaGetClipOptions(
[property: CommandSwitch("--clip-fragment-selector")] string ClipFragmentSelector
) : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }
}