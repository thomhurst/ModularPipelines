using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "update")]
public record AzAksarcUpdateOptions(
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