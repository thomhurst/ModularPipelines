using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs", "create-recording-configuration")]
public record AwsIvsCreateRecordingConfigurationOptions(
[property: CommandSwitch("--destination-configuration")] string DestinationConfiguration
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--recording-reconnect-window-seconds")]
    public int? RecordingReconnectWindowSeconds { get; set; }

    [CommandSwitch("--rendition-configuration")]
    public string? RenditionConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--thumbnail-configuration")]
    public string? ThumbnailConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}