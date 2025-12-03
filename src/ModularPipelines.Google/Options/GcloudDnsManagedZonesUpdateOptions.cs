using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "managed-zones", "update")]
public record GcloudDnsManagedZonesUpdateOptions(
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--denial-of-existence")]
    public string? DenialOfExistence { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dnssec-state")]
    public string? DnssecState { get; set; }

    [CliOption("--forwarding-targets")]
    public string[]? ForwardingTargets { get; set; }

    [CliOption("--gkeclusters")]
    public string[]? Gkeclusters { get; set; }

    [CliOption("--ksk-algorithm")]
    public string? KskAlgorithm { get; set; }

    [CliOption("--ksk-key-length")]
    public string? KskKeyLength { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--[no-]log-dns-queries")]
    public string[]? NoLogDnsQueries { get; set; }

    [CliFlag("--managed-reverse-lookup")]
    public bool? ManagedReverseLookup { get; set; }

    [CliOption("--networks")]
    public string[]? Networks { get; set; }

    [CliOption("--private-forwarding-targets")]
    public string[]? PrivateForwardingTargets { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--zsk-algorithm")]
    public string? ZskAlgorithm { get; set; }

    [CliOption("--zsk-key-length")]
    public string? ZskKeyLength { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--target-network")]
    public string? TargetNetwork { get; set; }

    [CliOption("--target-project")]
    public string? TargetProject { get; set; }
}