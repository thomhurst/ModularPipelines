using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "managed-zones", "update")]
public record GcloudDnsManagedZonesUpdateOptions(
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--denial-of-existence")]
    public string? DenialOfExistence { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

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

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--zsk-algorithm")]
    public string? ZskAlgorithm { get; set; }

    [CommandSwitch("--zsk-key-length")]
    public string? ZskKeyLength { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--target-network")]
    public string? TargetNetwork { get; set; }

    [CommandSwitch("--target-project")]
    public string? TargetProject { get; set; }
}