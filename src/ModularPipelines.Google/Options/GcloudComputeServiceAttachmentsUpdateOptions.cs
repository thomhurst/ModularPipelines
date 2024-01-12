using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "service-attachments", "update")]
public record GcloudComputeServiceAttachmentsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--connection-preference")]
    public string? ConnectionPreference { get; set; }

    [CommandSwitch("--consumer-accept-list")]
    public string[]? ConsumerAcceptList { get; set; }

    [CommandSwitch("--consumer-reject-list")]
    public string[]? ConsumerRejectList { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--[no-]enable-proxy-protocol")]
    public string[]? NoEnableProxyProtocol { get; set; }

    [CommandSwitch("--nat-subnets")]
    public string[]? NatSubnets { get; set; }

    [CommandSwitch("--nat-subnets-region")]
    public string? NatSubnetsRegion { get; set; }

    [CommandSwitch("--[no-]reconcile-connections")]
    public string[]? NoReconcileConnections { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}