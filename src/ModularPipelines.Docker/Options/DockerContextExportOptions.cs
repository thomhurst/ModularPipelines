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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? ExportContext { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? File { get; set; }
}
