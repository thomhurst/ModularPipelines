using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContextCreateOptions : DockerOptions
{
    public DockerContextCreateOptions(
        string context
    )
    {
        CommandParts = ["context", "create"];

        CreateContext = context;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? CreateContext { get; set; }

    [CliOption("--description")]
    public virtual string? Description { get; set; }

    [CliOption("--docker")]
    public virtual string? Docker { get; set; }

    [CliOption("--from")]
    public virtual string? From { get; set; }
}
