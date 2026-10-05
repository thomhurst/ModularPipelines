using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Git.Options;

[ExcludeFromCodeCoverage]
[CliTool("git")]
[CliGlobalOptions]
public record GitOptions : CommandLineToolOptions
{
    /// <summary>Changes directory before command execution. Relative entries resolve against the preceding entry.</summary>
    [CliOption("-C", Phase = CommandLinePhase.EarlyOperand)]
    public virtual string[]? ChangeDirectories { get; set; }

    /// <summary>Overrides configuration in order. Values are masked because configuration can contain credentials.</summary>
    [CliOption("-c")]
    [SecretValue]
    public virtual KeyValue[]? Configuration { get; set; }

    [CliOption("--config-env", Format = OptionFormat.EqualsSeparated)]
    public virtual string[]? ConfigEnv { get; set; }

    [CliOption("--exec-path", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ExecPath { get; set; }

    [CliFlag("--paginate")]
    public virtual bool? Paginate { get; set; }

    [CliFlag("--no-pager")]
    public virtual bool? NoPager { get; set; }

    [CliOption("--git-dir", Format = OptionFormat.EqualsSeparated)]
    public virtual string? GitDirectory { get; set; }

    [CliOption("--work-tree", Format = OptionFormat.EqualsSeparated)]
    public virtual string? WorkTree { get; set; }

    [CliOption("--namespace", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Namespace { get; set; }

    [CliFlag("--bare")]
    public virtual bool? BareRepository { get; set; }

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

    [CliOption("--attr-source", Format = OptionFormat.EqualsSeparated)]
    public virtual string? AttrSource { get; set; }

    [CliFlag("--no-lazy-fetch")]
    public virtual bool? NoLazyFetch { get; set; }

    [CliFlag("--no-advice")]
    public virtual bool? NoAdvice { get; set; }
}
