using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImagePullOptions : DockerOptions
{
    public DockerImagePullOptions(
        string name
    )
    {
        CommandParts = ["image", "pull"];

        Name = name;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Name { get; set; }

    [CliFlag("--all-tags")]
    public virtual bool? AllTags { get; set; }

    [CliFlag("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
