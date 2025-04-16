using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("rev-parse")]
[ExcludeFromCodeCoverage]
public record GitRevParseOptions : GitOptions
{
    [BooleanCommandSwitch("--parseopt")]
    public virtual bool? Parseopt { get; set; }

    [BooleanCommandSwitch("--sq-quote")]
    public virtual bool? SqQuote { get; set; }

    [BooleanCommandSwitch("--keep-dashdash")]
    public virtual bool? KeepDashdash { get; set; }

    [BooleanCommandSwitch("--stop-at-non-option")]
    public virtual bool? StopAtNonOption { get; set; }

    [BooleanCommandSwitch("--stuck-long")]
    public virtual bool? StuckLong { get; set; }

    [BooleanCommandSwitch("--revs-only")]
    public virtual bool? RevsOnly { get; set; }

    [BooleanCommandSwitch("--no-revs")]
    public virtual bool? NoRevs { get; set; }

    [BooleanCommandSwitch("--flags")]
    public virtual bool? Flags { get; set; }

    [BooleanCommandSwitch("--no-flags")]
    public virtual bool? NoFlags { get; set; }

    [CommandEqualsSeparatorSwitch("--default")]
    public string? Default { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--sq")]
    public virtual bool? Sq { get; set; }

    [BooleanCommandSwitch("--short")]
    public virtual bool? Short { get; set; }

    [BooleanCommandSwitch("--not")]
    public virtual bool? Not { get; set; }

    [BooleanCommandSwitch("--abbrev-ref")]
    public virtual bool? AbbrevRef { get; set; }

    [BooleanCommandSwitch("--symbolic")]
    public virtual bool? Symbolic { get; set; }

    [BooleanCommandSwitch("--symbolic-full-name")]
    public virtual bool? SymbolicFullName { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--branches")]
    public virtual bool? Branches { get; set; }

    [BooleanCommandSwitch("--tags")]
    public virtual bool? Tags { get; set; }

    [BooleanCommandSwitch("--remotes")]
    public virtual bool? Remotes { get; set; }

    [BooleanCommandSwitch("--glob")]
    public virtual bool? Glob { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--exclude-hidden")]
    public virtual bool? ExcludeHidden { get; set; }

    [CommandEqualsSeparatorSwitch("--disambiguate")]
    public string? Disambiguate { get; set; }

    [BooleanCommandSwitch("--local-env-vars")]
    public virtual bool? LocalEnvVars { get; set; }

    [BooleanCommandSwitch("--path-format")]
    public virtual bool? PathFormat { get; set; }

    [BooleanCommandSwitch("--git-dir")]
    public virtual bool? GitDir { get; set; }

    [BooleanCommandSwitch("--git-common-dir")]
    public virtual bool? GitCommonDir { get; set; }

    [CommandEqualsSeparatorSwitch("--resolve-git-dir")]
    public string? ResolveGitDir { get; set; }

    [CommandEqualsSeparatorSwitch("--git-path")]
    public string? GitPath { get; set; }

    [BooleanCommandSwitch("--show-toplevel")]
    public virtual bool? ShowToplevel { get; set; }

    [BooleanCommandSwitch("--show-superproject-working-tree")]
    public virtual bool? ShowSuperprojectWorkingTree { get; set; }

    [BooleanCommandSwitch("--shared-index-path")]
    public virtual bool? SharedIndexPath { get; set; }

    [BooleanCommandSwitch("--absolute-git-dir")]
    public virtual bool? AbsoluteGitDir { get; set; }

    [BooleanCommandSwitch("--is-inside-git-dir")]
    public virtual bool? IsInsideGitDir { get; set; }

    [BooleanCommandSwitch("--is-inside-work-tree")]
    public virtual bool? IsInsideWorkTree { get; set; }

    [BooleanCommandSwitch("--is-bare-repository")]
    public virtual bool? IsBareRepository { get; set; }

    [BooleanCommandSwitch("--is-shallow-repository")]
    public virtual bool? IsShallowRepository { get; set; }

    [BooleanCommandSwitch("--show-cdup")]
    public virtual bool? ShowCdup { get; set; }

    [BooleanCommandSwitch("--show-prefix")]
    public virtual bool? ShowPrefix { get; set; }

    [BooleanCommandSwitch("--show-object-format")]
    public virtual bool? ShowObjectFormat { get; set; }

    [BooleanCommandSwitch("--since")]
    public virtual bool? Since { get; set; }

    [BooleanCommandSwitch("--after")]
    public virtual bool? After { get; set; }

    [BooleanCommandSwitch("--until")]
    public virtual bool? Until { get; set; }

    [BooleanCommandSwitch("--before")]
    public virtual bool? Before { get; set; }
}