using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginCreateOptions : DockerOptions
{
    public DockerPluginCreateOptions(
        string plugin,
        string pluginDataDir
    )
    {
        CommandParts = ["plugin", "create"];

        Plugin = plugin;

        PluginDataDir = pluginDataDir;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Plugin { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? PluginDataDir { get; set; }

    [CliFlag("--compress")]
    public virtual bool? Compress { get; set; }
}
