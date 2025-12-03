using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginUpgradeOptions : DockerOptions
{
    public DockerPluginUpgradeOptions(
        string plugin
    )
    {
        CommandParts = ["plugin", "upgrade"];

        Plugin = plugin;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Plugin { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Remote { get; set; }

    [CliFlag("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CliOption("--grant-all-permissions")]
    public virtual string? GrantAllPermissions { get; set; }

    [CliOption("--skip-remote-check")]
    public virtual string? SkipRemoteCheck { get; set; }
}
