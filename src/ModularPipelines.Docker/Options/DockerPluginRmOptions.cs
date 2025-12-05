using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginRmOptions : DockerOptions
{
    public DockerPluginRmOptions(
        IEnumerable<string> plugin
    )
    {
        CommandParts = ["plugin", "rm"];

        Plugin = plugin;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Plugin { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
