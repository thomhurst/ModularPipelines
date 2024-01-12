using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "managed-zones", "create")]
public record GcloudDnsManagedZonesCreateOptions(
[property: PositionalArgument] string ZoneName,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--dns-name")] string DnsName
) : GcloudOptions
{
    [CommandSwitch("--denial-of-existence")]
    public string? DenialOfExistence { get; set; }

    [CommandSwitch("--dnssec-state")]
    public string? DnssecState { get; set; }

    [CommandSwitch("--forwarding-targets")]
    public string[]? ForwardingTargets { get; set; }

    [CommandSwitch("--gkeclusters")]
    public string[]? Gkeclusters { get; set; }

    [CommandSwitch("--ksk-algorithm")]
    public string? KskAlgorithm { get; set; }

    [CommandSwitch("--ksk-key-length")]
    public string? KskKeyLength { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--[no-]log-dns-queries")]
    public string[]? NoLogDnsQueries { get; set; }

    [BooleanCommandSwitch("--managed-reverse-lookup")]
    public bool? ManagedReverseLookup { get; set; }

    [CommandSwitch("--networks")]
    public string[]? Networks { get; set; }

    [CommandSwitch("--private-forwarding-targets")]
    public string[]? PrivateForwardingTargets { get; set; }

    [CommandSwitch("--service-directory-namespace")]
    public string? ServiceDirectoryNamespace { get; set; }

    [CommandSwitch("--visibility")]
    public string? Visibility { get; set; }

    [CommandSwitch("--zsk-algorithm")]
    public string? ZskAlgorithm { get; set; }

    [CommandSwitch("--zsk-key-length")]
    public string? ZskKeyLength { get; set; }

    [CommandSwitch("--target-network")]
    public string? TargetNetwork { get; set; }

    [CommandSwitch("--target-project")]
    public string? TargetProject { get; set; }
}