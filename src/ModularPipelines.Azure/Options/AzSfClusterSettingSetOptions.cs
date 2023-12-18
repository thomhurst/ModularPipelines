using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "setting", "set")]
public record AzSfClusterSettingSetOptions(
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

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}