using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "setting", "remove")]
public record AzSfClusterSettingRemoveOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--parameter")]
    public string? Parameter { get; set; }

    [CommandSwitch("--section")]
    public string? Section { get; set; }

    [CommandSwitch("--settings-section")]
    public string? SettingsSection { get; set; }
}