using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerTrustKeyLoadOptions : DockerOptions
{
    public DockerTrustKeyLoadOptions(
        string keyfile
    )
    {
        CommandParts = ["trust", "key", "load"];

        Keyfile = keyfile;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Keyfile { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }
}
