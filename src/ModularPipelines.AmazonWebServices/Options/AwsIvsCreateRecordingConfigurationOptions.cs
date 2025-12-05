using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs", "create-recording-configuration")]
public record AwsIvsCreateRecordingConfigurationOptions(
[property: CliOption("--destination-configuration")] string DestinationConfiguration
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--recording-reconnect-window-seconds")]
    public int? RecordingReconnectWindowSeconds { get; set; }

    [CliOption("--rendition-configuration")]
    public string? RenditionConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--thumbnail-configuration")]
    public string? ThumbnailConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}