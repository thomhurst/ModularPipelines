using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-node-type", "vm-extension", "add")]
public record AzSfManagedNodeTypeVmExtensionAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--extension-name")] string ExtensionName,
[property: CliOption("--extension-type")] string ExtensionType,
[property: CliOption("--name")] string Name,
[property: CliOption("--publisher")] string Publisher,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type-handler-version")] string TypeHandlerVersion
) : AzOptions
{
    [CliFlag("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [CliFlag("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CliOption("--protected-setting")]
    public string? ProtectedSetting { get; set; }

    [CliOption("--provision-after")]
    public string? ProvisionAfter { get; set; }

    [CliOption("--setting")]
    public string? Setting { get; set; }
}