using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-cluster", "network-security-rule", "add")]
public record AzSfManagedClusterNetworkSecurityRuleAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--access")]
    public string? Access { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dest-addr-prefixes")]
    public string? DestAddrPrefixes { get; set; }

    [CliOption("--dest-port-ranges")]
    public string? DestPortRanges { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--source-addr-prefixes")]
    public string? SourceAddrPrefixes { get; set; }

    [CliOption("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }
}