using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "external-access-rules", "update")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesUpdateOptions(
[property: CliArgument] string ExternalAccessRule,
[property: CliArgument] string Location,
[property: CliArgument] string NetworkPolicy
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

    [CliOption("--destination-ranges")]
    public string[]? DestinationRanges { get; set; }

    [CliOption("--ip-protocol")]
    public string? IpProtocol { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--source-ports")]
    public string[]? SourcePorts { get; set; }

    [CliOption("--source-ranges")]
    public string[]? SourceRanges { get; set; }
}