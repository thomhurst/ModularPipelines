using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("diff")]
public record NpmDiffOptions : NpmOptions
{
    [CommandSwitch("--diff")]
    public virtual string[]? Diff { get; set; }

    [BooleanCommandSwitch("--diff-name-only")]
    public virtual bool? DiffNameOnly { get; set; }

    [CommandSwitch("--diff-unified")]
    public virtual int? DiffUnified { get; set; }

    [BooleanCommandSwitch("--diff-ignore-all-space")]
    public virtual bool? DiffIgnoreAllSpace { get; set; }

    [BooleanCommandSwitch("--diff-no-prefix")]
    public virtual bool? DiffNoPrefix { get; set; }

    [CommandSwitch("--diff-src-prefix")]
    public virtual string? DiffSrcPrefix { get; set; }

    [CommandSwitch("--diff-dst-prefix")]
    public virtual string? DiffDstPrefix { get; set; }

    [BooleanCommandSwitch("--diff-text")]
    public virtual bool? DiffText { get; set; }

    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }

    [CommandSwitch("--tag")]
    public virtual string? Tag { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Paths { get; set; }
}