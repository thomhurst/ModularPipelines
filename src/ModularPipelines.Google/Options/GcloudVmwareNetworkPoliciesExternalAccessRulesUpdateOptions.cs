using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "external-access-rules", "update")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesUpdateOptions(
[property: PositionalArgument] string ExternalAccessRule,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string NetworkPolicy
) : GcloudOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination-ports")]
    public string[]? DestinationPorts { get; set; }

    [CommandSwitch("--destination-ranges")]
    public string[]? DestinationRanges { get; set; }

    [CommandSwitch("--ip-protocol")]
    public string? IpProtocol { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--source-ports")]
    public string[]? SourcePorts { get; set; }

    [CommandSwitch("--source-ranges")]
    public string[]? SourceRanges { get; set; }
}