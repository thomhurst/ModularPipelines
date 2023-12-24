using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-flow-source")]
public record AwsMediaconnectUpdateFlowSourceOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--source-arn")] string SourceArn
) : AwsOptions
{
    [CommandSwitch("--decryption")]
    public string? Decryption { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--entitlement-arn")]
    public string? EntitlementArn { get; set; }

    [CommandSwitch("--ingest-port")]
    public int? IngestPort { get; set; }

    [CommandSwitch("--max-bitrate")]
    public int? MaxBitrate { get; set; }

    [CommandSwitch("--max-latency")]
    public int? MaxLatency { get; set; }

    [CommandSwitch("--max-sync-buffer")]
    public int? MaxSyncBuffer { get; set; }

    [CommandSwitch("--media-stream-source-configurations")]
    public string[]? MediaStreamSourceConfigurations { get; set; }

    [CommandSwitch("--min-latency")]
    public int? MinLatency { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--sender-control-port")]
    public int? SenderControlPort { get; set; }

    [CommandSwitch("--sender-ip-address")]
    public string? SenderIpAddress { get; set; }

    [CommandSwitch("--source-listener-address")]
    public string? SourceListenerAddress { get; set; }

    [CommandSwitch("--source-listener-port")]
    public int? SourceListenerPort { get; set; }

    [CommandSwitch("--stream-id")]
    public string? StreamId { get; set; }

    [CommandSwitch("--vpc-interface-name")]
    public string? VpcInterfaceName { get; set; }

    [CommandSwitch("--whitelist-cidr")]
    public string? WhitelistCidr { get; set; }

    [CommandSwitch("--gateway-bridge-source")]
    public string? GatewayBridgeSource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}