using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackagev2", "create-origin-endpoint")]
public record AwsMediapackagev2CreateOriginEndpointOptions(
[property: CommandSwitch("--channel-group-name")] string ChannelGroupName,
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--origin-endpoint-name")] string OriginEndpointName,
[property: CommandSwitch("--container-type")] string ContainerType
) : AwsOptions
{
    [CommandSwitch("--segment")]
    public string? Segment { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--startover-window-seconds")]
    public int? StartoverWindowSeconds { get; set; }

    [CommandSwitch("--hls-manifests")]
    public string[]? HlsManifests { get; set; }

    [CommandSwitch("--low-latency-hls-manifests")]
    public string[]? LowLatencyHlsManifests { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}