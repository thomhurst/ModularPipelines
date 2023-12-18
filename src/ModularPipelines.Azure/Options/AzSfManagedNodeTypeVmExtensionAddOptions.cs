using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-node-type", "vm-extension", "add")]
public record AzSfManagedNodeTypeVmExtensionAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--extension-name")] string ExtensionName,
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--publisher")] string Publisher,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type-handler-version")] string TypeHandlerVersion
) : AzOptions
{
    [BooleanCommandSwitch("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [BooleanCommandSwitch("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CommandSwitch("--protected-setting")]
    public string? ProtectedSetting { get; set; }

    [CommandSwitch("--provision-after")]
    public string? ProvisionAfter { get; set; }

    [CommandSwitch("--setting")]
    public string? Setting { get; set; }
}