using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("diff")]
public record NpmDiffOptions : NpmOptions
{
    [CliOption("--diff")]
    public virtual string[]? Diff { get; set; }

    [CliFlag("--diff-name-only")]
    public virtual bool? DiffNameOnly { get; set; }

    [CliOption("--diff-unified")]
    public virtual int? DiffUnified { get; set; }

    [CliFlag("--diff-ignore-all-space")]
    public virtual bool? DiffIgnoreAllSpace { get; set; }

    [CliFlag("--diff-no-prefix")]
    public virtual bool? DiffNoPrefix { get; set; }

    [CliOption("--diff-src-prefix")]
    public virtual string? DiffSrcPrefix { get; set; }

    [CliOption("--diff-dst-prefix")]
    public virtual string? DiffDstPrefix { get; set; }

    [CliFlag("--diff-text")]
    public virtual bool? DiffText { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--tag")]
    public virtual string? Tag { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Paths { get; set; }
}