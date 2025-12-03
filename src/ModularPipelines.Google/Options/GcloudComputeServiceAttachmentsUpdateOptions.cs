using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "service-attachments", "update")]
public record GcloudComputeServiceAttachmentsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--connection-preference")]
    public string? ConnectionPreference { get; set; }

    [CliOption("--consumer-accept-list")]
    public string[]? ConsumerAcceptList { get; set; }

    [CliOption("--consumer-reject-list")]
    public string[]? ConsumerRejectList { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--[no-]enable-proxy-protocol")]
    public string[]? NoEnableProxyProtocol { get; set; }

    [CliOption("--nat-subnets")]
    public string[]? NatSubnets { get; set; }

    [CliOption("--nat-subnets-region")]
    public string? NatSubnetsRegion { get; set; }

    [CliOption("--[no-]reconcile-connections")]
    public string[]? NoReconcileConnections { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}