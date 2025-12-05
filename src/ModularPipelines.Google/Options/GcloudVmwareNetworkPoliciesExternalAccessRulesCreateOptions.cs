using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "external-access-rules", "create")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesCreateOptions(
[property: CliArgument] string ExternalAccessRule,
[property: CliArgument] string Location,
[property: CliArgument] string NetworkPolicy,
[property: CliOption("--destination-ranges")] string[] DestinationRanges,
[property: CliOption("--ip-protocol")] string IpProtocol,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--source-ranges")] string[] SourceRanges
) : GcloudOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination-ports")]
    public string[]? DestinationPorts { get; set; }

    [CliOption("--source-ports")]
    public string[]? SourcePorts { get; set; }
}