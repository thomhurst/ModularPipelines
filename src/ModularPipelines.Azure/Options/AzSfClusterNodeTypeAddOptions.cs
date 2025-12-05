using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "cluster", "node-type", "add")]
public record AzSfClusterNodeTypeAddOptions(
[property: CliOption("--capacity")] string Capacity,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-password")] string VmPassword,
[property: CliOption("--vm-user-name")] string VmUserName
) : AzOptions
{
    [CliOption("--durability-level")]
    public string? DurabilityLevel { get; set; }

    [CliOption("--vm-sku")]
    public string? VmSku { get; set; }

    [CliOption("--vm-tier")]
    public string? VmTier { get; set; }
}