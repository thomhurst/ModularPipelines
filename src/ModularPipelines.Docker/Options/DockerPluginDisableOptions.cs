using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginDisableOptions : DockerOptions
{
    public DockerPluginDisableOptions(
        string plugin
    )
    {
        CommandParts = ["plugin", "disable"];

        Plugin = plugin;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Plugin { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
