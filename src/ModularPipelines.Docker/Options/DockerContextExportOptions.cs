using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContextExportOptions : DockerOptions
{
    public DockerContextExportOptions(
        string context
    )
    {
        CommandParts = ["context", "export"];

        ExportContext = context;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ExportContext { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? File { get; set; }
}
