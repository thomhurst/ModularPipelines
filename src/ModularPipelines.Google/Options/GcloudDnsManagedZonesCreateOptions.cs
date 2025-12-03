using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "managed-zones", "create")]
public record GcloudDnsManagedZonesCreateOptions(
[property: CliArgument] string ZoneName,
[property: CliOption("--description")] string Description,
[property: CliOption("--dns-name")] string DnsName
) : GcloudOptions
{
    [CliOption("--denial-of-existence")]
    public string? DenialOfExistence { get; set; }

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

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

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

    [CliOption("--service-directory-namespace")]
    public string? ServiceDirectoryNamespace { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }

    [CliOption("--zsk-algorithm")]
    public string? ZskAlgorithm { get; set; }

    [CliOption("--zsk-key-length")]
    public string? ZskKeyLength { get; set; }

    [CliOption("--target-network")]
    public string? TargetNetwork { get; set; }

    [CliOption("--target-project")]
    public string? TargetProject { get; set; }
}