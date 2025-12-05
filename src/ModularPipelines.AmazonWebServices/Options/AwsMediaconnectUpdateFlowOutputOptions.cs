using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-flow-output")]
public record AwsMediaconnectUpdateFlowOutputOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--output-arn")] string OutputArn
) : AwsOptions
{
    [CliOption("--cidr-allow-list")]
    public string[]? CidrAllowList { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--max-latency")]
    public int? MaxLatency { get; set; }

    [CliOption("--media-stream-output-configurations")]
    public string[]? MediaStreamOutputConfigurations { get; set; }

    [CliOption("--min-latency")]
    public int? MinLatency { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--remote-id")]
    public string? RemoteId { get; set; }

    [CliOption("--sender-control-port")]
    public int? SenderControlPort { get; set; }

    [CliOption("--sender-ip-address")]
    public string? SenderIpAddress { get; set; }

    [CliOption("--smoothing-latency")]
    public int? SmoothingLatency { get; set; }

    [CliOption("--stream-id")]
    public string? StreamId { get; set; }

    [CliOption("--vpc-interface-attachment")]
    public string? VpcInterfaceAttachment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}