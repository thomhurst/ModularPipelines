using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "create")]
public record GcloudVmwarePrivateCloudsCreateOptions(
[property: PositionalArgument] string PrivateCloud,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--management-range")] string ManagementRange,
[property: CommandSwitch("--node-type-config")] string[] NodeTypeConfig,
[property: CommandSwitch("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--preferred-zone")]
    public string? PreferredZone { get; set; }

    [CommandSwitch("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}