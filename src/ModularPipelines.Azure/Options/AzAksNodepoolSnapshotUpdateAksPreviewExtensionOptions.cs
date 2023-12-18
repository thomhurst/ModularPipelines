using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "snapshot", "update", "(aks-preview", "extension)")]
public record AzAksNodepoolSnapshotUpdateAksPreviewExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--tags")] string Tags
) : AzOptions
{
    [BooleanCommandSwitch("--created")]
    public bool? Created { get; set; }

    [CommandSwitch("--custom")]
    public string? Custom { get; set; }

    [BooleanCommandSwitch("--deleted")]
    public bool? Deleted { get; set; }

    [BooleanCommandSwitch("--exists")]
    public bool? Exists { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--updated")]
    public bool? Updated { get; set; }
}

