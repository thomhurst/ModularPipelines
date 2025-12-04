using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "create")]
public record AzNetworkFrontDoorCreateOptions(
[property: CliOption("--backend-address")] string BackendAddress,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--accepted-protocols")]
    public string? AcceptedProtocols { get; set; }

    [CliOption("--backend-host-header")]
    public string? BackendHostHeader { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--enforce-certificate-name-check")]
    public string? EnforceCertificateNameCheck { get; set; }

    [CliOption("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--frontend-host-name")]
    public string? FrontendHostName { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--patterns")]
    public string? Patterns { get; set; }

    [CliOption("--probeMethod")]
    public string? ProbeMethod { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--send-recv-timeout")]
    public string? SendRecvTimeout { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}