using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("publish")]
public record NpmPublishOptions : NpmOptions
{
    [CommandSwitch("--tag")]
    public virtual string? Tag { get; set; }

    [CommandSwitch("--access")]
    public virtual string? Access { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--provenance")]
    public virtual bool? Provenance { get; set; }

    [CommandSwitch("--provenance-file")]
    public virtual string? ProvenanceFile { get; set; }
}