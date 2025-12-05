using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginEnableOptions : DockerOptions
{
    public DockerPluginEnableOptions(
        string plugin
    )
    {
        CommandParts = ["plugin", "enable"];

        Plugin = plugin;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Plugin { get; set; }

    [CliOption("--timeout")]
    public virtual int? Timeout { get; set; }
}
