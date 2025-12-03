using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "sbom")]
[ExcludeFromCodeCoverage]
public record DockerScoutSbomOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? ImageOrDirectoryOrArchive { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--only-package-type")]
    public virtual string? OnlyPackageType { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--ref")]
    public virtual string? Ref { get; set; }
}
