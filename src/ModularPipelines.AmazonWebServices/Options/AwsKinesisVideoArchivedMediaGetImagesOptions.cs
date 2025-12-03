using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis-video-archived-media", "get-images")]
public record AwsKinesisVideoArchivedMediaGetImagesOptions(
[property: CliOption("--image-selector-type")] string ImageSelectorType,
[property: CliOption("--start-timestamp")] long StartTimestamp,
[property: CliOption("--end-timestamp")] long EndTimestamp,
[property: CliOption("--format")] string Format
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--sampling-interval")]
    public int? SamplingInterval { get; set; }

    [CliOption("--format-config")]
    public IEnumerable<KeyValue>? FormatConfig { get; set; }

    [CliOption("--width-pixels")]
    public int? WidthPixels { get; set; }

    [CliOption("--height-pixels")]
    public int? HeightPixels { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public long? PageSize { get; set; }

    [CliOption("--max-items")]
    public long? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}