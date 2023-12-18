using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "create")]
public record AzNetworkFrontDoorCreateOptions(
[property: CommandSwitch("--backend-address")] string BackendAddress,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--accepted-protocols")]
    public string? AcceptedProtocols { get; set; }

    [CommandSwitch("--backend-host-header")]
    public string? BackendHostHeader { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--enforce-certificate-name-check")]
    public string? EnforceCertificateNameCheck { get; set; }

    [CommandSwitch("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--frontend-host-name")]
    public string? FrontendHostName { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--patterns")]
    public string? Patterns { get; set; }

    [CommandSwitch("--probeMethod")]
    public string? ProbeMethod { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--send-recv-timeout")]
    public string? SendRecvTimeout { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}