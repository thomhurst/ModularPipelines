using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[ExcludeFromCodeCoverage]
public record GitBaseOptions : GitOptions
{
    [CliFlag("--version")]
    public virtual bool? Version { get; set; }

    [CliOption("--config-env", Format = OptionFormat.EqualsSeparated)]
    public virtual string[]? ConfigEnv { get; set; }

    [CliOption("--exec-path", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ExecPath { get; set; }

    [CliFlag("--html-path")]
    public virtual bool? HtmlPath { get; set; }

    [CliFlag("--man-path")]
    public virtual bool? ManPath { get; set; }

    [CliFlag("--info-path")]
    public virtual bool? InfoPath { get; set; }

    [CliFlag("--paginate")]
    public virtual bool? Paginate { get; set; }

    [CliFlag("--no-pager")]
    public virtual bool? NoPager { get; set; }

    [CliOption("--git-dir", Format = OptionFormat.EqualsSeparated)]
    public virtual string? GitDir { get; set; }

    [CliOption("--work-tree", Format = OptionFormat.EqualsSeparated)]
    public virtual string? WorkTree { get; set; }

    [CliOption("--namespace", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Namespace { get; set; }

    [CliFlag("--bare")]
    public virtual bool? Bare { get; set; }

    [CliFlag("--no-replace-objects")]
    public virtual bool? NoReplaceObjects { get; set; }

    [CliFlag("--literal-pathspecs")]
    public virtual bool? LiteralPathspecs { get; set; }

    [CliFlag("--glob-pathspecs")]
    public virtual bool? GlobPathspecs { get; set; }

    [CliFlag("--noglob-pathspecs")]
    public virtual bool? NoglobPathspecs { get; set; }

    [CliFlag("--icase-pathspecs")]
    public virtual bool? IcasePathspecs { get; set; }

    [CliFlag("--no-optional-locks")]
    public virtual bool? NoOptionalLocks { get; set; }

    [CliFlag("--list-cmds")]
    public virtual bool? ListCmds { get; set; }

    [CliOption("--attr-source", Format = OptionFormat.EqualsSeparated)]
    public virtual string? AttrSource { get; set; }
}