using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginInspectOptions : DockerOptions
{
    public DockerPluginInspectOptions(
        IEnumerable<string> plugin
    )
    {
        CommandParts = ["plugin", "inspect"];

        Plugin = plugin;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Plugin { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }
}
