using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImagePushOptions : DockerOptions
{
    public DockerImagePushOptions(
        string name
    )
    {
        CommandParts = ["image", "push"];

        Name = name;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Name { get; set; }

    [CliFlag("--all-tags")]
    public virtual bool? AllTags { get; set; }

    [CliFlag("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
