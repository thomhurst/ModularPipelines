using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sbom")]
public record NpmSbomOptions : NpmOptions
{
    [CommandSwitch("--omit")]
    public virtual string? Omit { get; set; }

    [BooleanCommandSwitch("--package-lock-only")]
    public virtual bool? PackageLockOnly { get; set; }

    [CommandSwitch("--sbom-format")]
    public virtual string? SbomFormat { get; set; }

    [CommandSwitch("--sbom-type")]
    public virtual string? SbomType { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }
}