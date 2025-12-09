using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-flow-source")]
public record AwsMediaconnectUpdateFlowSourceOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--source-arn")] string SourceArn
) : AwsOptions
{
    [CliOption("--decryption")]
    public string? Decryption { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--entitlement-arn")]
    public string? EntitlementArn { get; set; }

    [CliOption("--ingest-port")]
    public int? IngestPort { get; set; }

    [CliOption("--max-bitrate")]
    public int? MaxBitrate { get; set; }

    [CliOption("--max-latency")]
    public int? MaxLatency { get; set; }

    [CliOption("--max-sync-buffer")]
    public int? MaxSyncBuffer { get; set; }

    [CliOption("--media-stream-source-configurations")]
    public string[]? MediaStreamSourceConfigurations { get; set; }

    [CliOption("--min-latency")]
    public int? MinLatency { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--sender-control-port")]
    public int? SenderControlPort { get; set; }

    [CliOption("--sender-ip-address")]
    public string? SenderIpAddress { get; set; }

    [CliOption("--source-listener-address")]
    public string? SourceListenerAddress { get; set; }

    [CliOption("--source-listener-port")]
    public int? SourceListenerPort { get; set; }

    [CliOption("--stream-id")]
    public string? StreamId { get; set; }

    [CliOption("--vpc-interface-name")]
    public string? VpcInterfaceName { get; set; }

    [CliOption("--whitelist-cidr")]
    public string? WhitelistCidr { get; set; }

    [CliOption("--gateway-bridge-source")]
    public string? GatewayBridgeSource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}