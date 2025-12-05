using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginInstallOptions : DockerOptions
{
    public DockerPluginInstallOptions(
        string plugin
    )
    {
        CommandParts = ["plugin", "install"];

        Plugin = plugin;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Plugin { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<KeyValue>? KeyValue { get; set; }

    [CliOption("--alias")]
    public virtual string? Alias { get; set; }

    [CliOption("--disable")]
    public virtual string? Disable { get; set; }

    [CliFlag("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CliOption("--grant-all-permissions")]
    public virtual string? GrantAllPermissions { get; set; }
}
