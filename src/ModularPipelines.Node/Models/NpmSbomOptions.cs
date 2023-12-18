using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sbom")]
public record NpmSbomOptions : NpmOptions
{
    [CommandSwitch("--omit")]
    public string? Omit { get; set; }

    [BooleanCommandSwitch("--package-lock-only")]
    public bool? PackageLockOnly { get; set; }

    [CommandSwitch("--sbom-format")]
    public string? SbomFormat { get; set; }

    [CommandSwitch("--sbom-type")]
    public string? SbomType { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }
}