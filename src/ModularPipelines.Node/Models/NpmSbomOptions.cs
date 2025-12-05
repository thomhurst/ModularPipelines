using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("sbom")]
public record NpmSbomOptions : NpmOptions
{
    [CliOption("--omit")]
    public virtual string? Omit { get; set; }

    [CliFlag("--package-lock-only")]
    public virtual bool? PackageLockOnly { get; set; }

    [CliOption("--sbom-format")]
    public virtual string? SbomFormat { get; set; }

    [CliOption("--sbom-type")]
    public virtual string? SbomType { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }
}