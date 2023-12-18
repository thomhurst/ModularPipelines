using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("diff")]
public record NpmDiffOptions : NpmOptions
{
    [CommandSwitch("--diff")]
    public string[]? Diff { get; set; }

    [BooleanCommandSwitch("--diff-name-only")]
    public bool? DiffNameOnly { get; set; }

    [CommandSwitch("--diff-unified")]
    public int? DiffUnified { get; set; }

    [BooleanCommandSwitch("--diff-ignore-all-space")]
    public bool? DiffIgnoreAllSpace { get; set; }

    [BooleanCommandSwitch("--diff-no-prefix")]
    public bool? DiffNoPrefix { get; set; }

    [CommandSwitch("--diff-src-prefix")]
    public string? DiffSrcPrefix { get; set; }

    [CommandSwitch("--diff-dst-prefix")]
    public string? DiffDstPrefix { get; set; }

    [BooleanCommandSwitch("--diff-text")]
    public bool? DiffText { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Paths { get; set; }
}