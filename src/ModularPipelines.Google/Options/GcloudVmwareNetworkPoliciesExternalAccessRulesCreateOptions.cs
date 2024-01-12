using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "external-access-rules", "create")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesCreateOptions(
[property: PositionalArgument] string ExternalAccessRule,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string NetworkPolicy,
[property: CommandSwitch("--destination-ranges")] string[] DestinationRanges,
[property: CommandSwitch("--ip-protocol")] string IpProtocol,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--source-ranges")] string[] SourceRanges
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

    [CommandSwitch("--source-ports")]
    public string[]? SourcePorts { get; set; }
}