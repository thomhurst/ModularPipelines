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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--all-tags")]
    public virtual bool? AllTags { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
