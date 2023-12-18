using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "show")]
public record AzAksarcShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CommandSwitch("--control-plane-node-count")]
    public int? ControlPlaneNodeCount { get; set; }

    [BooleanCommandSwitch("--disable-ahub")]
    public bool? DisableAhub { get; set; }

    [BooleanCommandSwitch("--enable-ahub")]
    public bool? EnableAhub { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

