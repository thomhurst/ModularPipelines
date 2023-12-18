using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "check-name-availability")]
public record AzNetworkFrontDoorCheckNameAvailabilityOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-type")] string ResourceType
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

