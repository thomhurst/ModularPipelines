using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-connections", "create")]
public record GcloudVmwarePrivateConnectionsCreateOptions(
[property: PositionalArgument] string PrivateConnection,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--service-project")] string ServiceProject,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--routing-mode")]
    public string? RoutingMode { get; set; }

    [CommandSwitch("--service-network")]
    public string? ServiceNetwork { get; set; }
}