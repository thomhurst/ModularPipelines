using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerInspectOptions : DockerOptions
{
    public DockerInspectOptions(
        IEnumerable<string> nameOrId
    )
    {
        CommandParts = ["inspect"];

        NameOrId = nameOrId;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? NameOrId { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--size")]
    public virtual string? Size { get; set; }

    [CliOption("--type")]
    public virtual string? Type { get; set; }
}
