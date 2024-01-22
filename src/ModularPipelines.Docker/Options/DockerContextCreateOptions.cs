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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? CreateContext { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--docker")]
    public string? Docker { get; set; }

    [CommandSwitch("--from")]
    public string? From { get; set; }
}
