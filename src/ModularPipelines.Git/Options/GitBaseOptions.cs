using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[ExcludeFromCodeCoverage]
public record GitBaseOptions : GitOptions
{
    [BooleanCommandSwitch("--version")]
    public virtual bool? Version { get; set; }

    [CommandEqualsSeparatorSwitch("--config-env")]
    public string[]? ConfigEnv { get; set; }

    [CommandEqualsSeparatorSwitch("--exec-path")]
    public string? ExecPath { get; set; }

    [BooleanCommandSwitch("--html-path")]
    public virtual bool? HtmlPath { get; set; }

    [BooleanCommandSwitch("--man-path")]
    public virtual bool? ManPath { get; set; }

    [BooleanCommandSwitch("--info-path")]
    public virtual bool? InfoPath { get; set; }

    [BooleanCommandSwitch("--paginate")]
    public virtual bool? Paginate { get; set; }

    [BooleanCommandSwitch("--no-pager")]
    public virtual bool? NoPager { get; set; }

    [CommandEqualsSeparatorSwitch("--git-dir")]
    public string? GitDir { get; set; }

    [CommandEqualsSeparatorSwitch("--work-tree")]
    public string? WorkTree { get; set; }

    [CommandEqualsSeparatorSwitch("--namespace")]
    public string? Namespace { get; set; }

    [BooleanCommandSwitch("--bare")]
    public virtual bool? Bare { get; set; }

    [BooleanCommandSwitch("--no-replace-objects")]
    public virtual bool? NoReplaceObjects { get; set; }

    [BooleanCommandSwitch("--literal-pathspecs")]
    public virtual bool? LiteralPathspecs { get; set; }

    [BooleanCommandSwitch("--glob-pathspecs")]
    public virtual bool? GlobPathspecs { get; set; }

    [BooleanCommandSwitch("--noglob-pathspecs")]
    public virtual bool? NoglobPathspecs { get; set; }

    [BooleanCommandSwitch("--icase-pathspecs")]
    public virtual bool? IcasePathspecs { get; set; }

    [BooleanCommandSwitch("--no-optional-locks")]
    public virtual bool? NoOptionalLocks { get; set; }

    [BooleanCommandSwitch("--list-cmds")]
    public virtual bool? ListCmds { get; set; }

    [CommandEqualsSeparatorSwitch("--attr-source")]
    public string? AttrSource { get; set; }
}