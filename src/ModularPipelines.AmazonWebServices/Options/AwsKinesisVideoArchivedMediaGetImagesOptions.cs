using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-archived-media", "get-images")]
public record AwsKinesisVideoArchivedMediaGetImagesOptions(
[property: CommandSwitch("--image-selector-type")] string ImageSelectorType,
[property: CommandSwitch("--start-timestamp")] long StartTimestamp,
[property: CommandSwitch("--end-timestamp")] long EndTimestamp,
[property: CommandSwitch("--format")] string Format
) : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--sampling-interval")]
    public int? SamplingInterval { get; set; }

    [CommandSwitch("--format-config")]
    public IEnumerable<KeyValue>? FormatConfig { get; set; }

    [CommandSwitch("--width-pixels")]
    public int? WidthPixels { get; set; }

    [CommandSwitch("--height-pixels")]
    public int? HeightPixels { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public long? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public long? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}