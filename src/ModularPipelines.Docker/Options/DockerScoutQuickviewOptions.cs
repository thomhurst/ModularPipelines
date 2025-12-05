using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "quickview")]
[ExcludeFromCodeCoverage]
public record DockerScoutQuickviewOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? ImageOrDirectoryOrArchive { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliOption("--latest")]
    public virtual string? Latest { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--ref")]
    public virtual string? Ref { get; set; }

    [CliOption("--stream")]
    public virtual string? Stream { get; set; }
}
