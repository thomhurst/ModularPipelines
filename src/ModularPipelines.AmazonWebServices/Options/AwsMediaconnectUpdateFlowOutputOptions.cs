using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-flow-output")]
public record AwsMediaconnectUpdateFlowOutputOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--output-arn")] string OutputArn
) : AwsOptions
{
    [CommandSwitch("--cidr-allow-list")]
    public string[]? CidrAllowList { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--max-latency")]
    public int? MaxLatency { get; set; }

    [CommandSwitch("--media-stream-output-configurations")]
    public string[]? MediaStreamOutputConfigurations { get; set; }

    [CommandSwitch("--min-latency")]
    public int? MinLatency { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--remote-id")]
    public string? RemoteId { get; set; }

    [CommandSwitch("--sender-control-port")]
    public int? SenderControlPort { get; set; }

    [CommandSwitch("--sender-ip-address")]
    public string? SenderIpAddress { get; set; }

    [CommandSwitch("--smoothing-latency")]
    public int? SmoothingLatency { get; set; }

    [CommandSwitch("--stream-id")]
    public string? StreamId { get; set; }

    [CommandSwitch("--vpc-interface-attachment")]
    public string? VpcInterfaceAttachment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}