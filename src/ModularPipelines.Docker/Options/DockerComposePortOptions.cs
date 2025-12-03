using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerComposePortOptions : DockerOptions
{
    public DockerComposePortOptions(
        string service,
        string privatePort
    )
    {
        CommandParts = ["compose", "port"];

        Service = service;

        PrivatePort = privatePort;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Service { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? PrivatePort { get; set; }

    [CliOption("--index")]
    public virtual string? Index { get; set; }

    [CliOption("--protocol")]
    public virtual string? Protocol { get; set; }
}
