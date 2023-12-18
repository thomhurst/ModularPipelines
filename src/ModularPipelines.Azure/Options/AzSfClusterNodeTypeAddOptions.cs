using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "node-type", "add")]
public record AzSfClusterNodeTypeAddOptions(
[property: CommandSwitch("--capacity")] string Capacity,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--node-type")] string NodeType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-password")] string VmPassword,
[property: CommandSwitch("--vm-user-name")] string VmUserName
) : AzOptions
{
    [CommandSwitch("--durability-level")]
    public string? DurabilityLevel { get; set; }

    [CommandSwitch("--vm-sku")]
    public string? VmSku { get; set; }

    [CommandSwitch("--vm-tier")]
    public string? VmTier { get; set; }
}