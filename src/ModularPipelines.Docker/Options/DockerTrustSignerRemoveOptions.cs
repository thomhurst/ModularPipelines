using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerTrustSignerRemoveOptions : DockerOptions
{
    public DockerTrustSignerRemoveOptions(
        string name,
        IEnumerable<string> repository
    )
    {
        CommandParts = ["trust", "signer", "remove"];

        Name = name;

        Repository = repository;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Name { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Repository { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
