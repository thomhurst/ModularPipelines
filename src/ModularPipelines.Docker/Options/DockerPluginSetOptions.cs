using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CliCommand("plugin", "set")]
[ExcludeFromCodeCoverage]
public record DockerPluginSetOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<KeyValue>? KeyValue { get; set; }
}
