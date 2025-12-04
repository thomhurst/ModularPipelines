using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("rev-parse")]
[ExcludeFromCodeCoverage]
public record GitRevParseOptions : GitOptions
{
    [CliFlag("--parseopt")]
    public virtual bool? Parseopt { get; set; }

    [CliFlag("--sq-quote")]
    public virtual bool? SqQuote { get; set; }

    [CliFlag("--keep-dashdash")]
    public virtual bool? KeepDashdash { get; set; }

    [CliFlag("--stop-at-non-option")]
    public virtual bool? StopAtNonOption { get; set; }

    [CliFlag("--stuck-long")]
    public virtual bool? StuckLong { get; set; }

    [CliFlag("--revs-only")]
    public virtual bool? RevsOnly { get; set; }

    [CliFlag("--no-revs")]
    public virtual bool? NoRevs { get; set; }

    [CliFlag("--flags")]
    public virtual bool? Flags { get; set; }

    [CliFlag("--no-flags")]
    public virtual bool? NoFlags { get; set; }

    [CliOption("--default", Format = OptionFormat.EqualsSeparated)]
    public string? Default { get; set; }

    [CliOption("--prefix", Format = OptionFormat.EqualsSeparated)]
    public string? Prefix { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--sq")]
    public virtual bool? Sq { get; set; }

    [CliFlag("--short")]
    public virtual bool? Short { get; set; }

    [CliFlag("--not")]
    public virtual bool? Not { get; set; }

    [CliFlag("--abbrev-ref")]
    public virtual bool? AbbrevRef { get; set; }

    [CliFlag("--symbolic")]
    public virtual bool? Symbolic { get; set; }

    [CliFlag("--symbolic-full-name")]
    public virtual bool? SymbolicFullName { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--branches")]
    public virtual bool? Branches { get; set; }

    [CliFlag("--tags")]
    public virtual bool? Tags { get; set; }

    [CliFlag("--remotes")]
    public virtual bool? Remotes { get; set; }

    [CliFlag("--glob")]
    public virtual bool? Glob { get; set; }

    [CliOption("--exclude", Format = OptionFormat.EqualsSeparated)]
    public string? Exclude { get; set; }

    [CliFlag("--exclude-hidden")]
    public virtual bool? ExcludeHidden { get; set; }

    [CliOption("--disambiguate", Format = OptionFormat.EqualsSeparated)]
    public string? Disambiguate { get; set; }

    [CliFlag("--local-env-vars")]
    public virtual bool? LocalEnvVars { get; set; }

    [CliFlag("--path-format")]
    public virtual bool? PathFormat { get; set; }

    [CliFlag("--git-dir")]
    public virtual bool? GitDir { get; set; }

    [CliFlag("--git-common-dir")]
    public virtual bool? GitCommonDir { get; set; }

    [CliOption("--resolve-git-dir", Format = OptionFormat.EqualsSeparated)]
    public string? ResolveGitDir { get; set; }

    [CliOption("--git-path", Format = OptionFormat.EqualsSeparated)]
    public string? GitPath { get; set; }

    [CliFlag("--show-toplevel")]
    public virtual bool? ShowToplevel { get; set; }

    [CliFlag("--show-superproject-working-tree")]
    public virtual bool? ShowSuperprojectWorkingTree { get; set; }

    [CliFlag("--shared-index-path")]
    public virtual bool? SharedIndexPath { get; set; }

    [CliFlag("--absolute-git-dir")]
    public virtual bool? AbsoluteGitDir { get; set; }

    [CliFlag("--is-inside-git-dir")]
    public virtual bool? IsInsideGitDir { get; set; }

    [CliFlag("--is-inside-work-tree")]
    public virtual bool? IsInsideWorkTree { get; set; }

    [CliFlag("--is-bare-repository")]
    public virtual bool? IsBareRepository { get; set; }

    [CliFlag("--is-shallow-repository")]
    public virtual bool? IsShallowRepository { get; set; }

    [CliFlag("--show-cdup")]
    public virtual bool? ShowCdup { get; set; }

    [CliFlag("--show-prefix")]
    public virtual bool? ShowPrefix { get; set; }

    [CliFlag("--show-object-format")]
    public virtual bool? ShowObjectFormat { get; set; }

    [CliFlag("--since")]
    public virtual bool? Since { get; set; }

    [CliFlag("--after")]
    public virtual bool? After { get; set; }

    [CliFlag("--until")]
    public virtual bool? Until { get; set; }

    [CliFlag("--before")]
    public virtual bool? Before { get; set; }
}