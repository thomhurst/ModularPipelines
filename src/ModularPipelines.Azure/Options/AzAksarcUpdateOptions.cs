using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aksarc", "update")]
public record AzAksarcUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CliOption("--control-plane-node-count")]
    public int? ControlPlaneNodeCount { get; set; }

    [CliFlag("--disable-ahub")]
    public bool? DisableAhub { get; set; }

    [CliFlag("--enable-ahub")]
    public bool? EnableAhub { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}