using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("rev-parse")]
public record GitRevParseOptions : GitOptions
{
    [BooleanCommandSwitch("--parseopt")]
    public bool? Parseopt { get; set; }

    [BooleanCommandSwitch("--sq-quote")]
    public bool? SqQuote { get; set; }

    [BooleanCommandSwitch("--keep-dashdash")]
    public bool? KeepDashdash { get; set; }

    [BooleanCommandSwitch("--stop-at-non-option")]
    public bool? StopAtNonOption { get; set; }

    [BooleanCommandSwitch("--stuck-long")]
    public bool? StuckLong { get; set; }

    [BooleanCommandSwitch("--revs-only")]
    public bool? RevsOnly { get; set; }

    [BooleanCommandSwitch("--no-revs")]
    public bool? NoRevs { get; set; }

    [BooleanCommandSwitch("--flags")]
    public bool? Flags { get; set; }

    [BooleanCommandSwitch("--no-flags")]
    public bool? NoFlags { get; set; }

    [CommandEqualsSeparatorSwitch("--default")]
    public string? Default { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--sq")]
    public bool? Sq { get; set; }

    [BooleanCommandSwitch("--short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("--not")]
    public bool? Not { get; set; }

    [BooleanCommandSwitch("--abbrev-ref")]
    public bool? AbbrevRef { get; set; }

    [BooleanCommandSwitch("--symbolic")]
    public bool? Symbolic { get; set; }

    [BooleanCommandSwitch("--symbolic-full-name")]
    public bool? SymbolicFullName { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--branches")]
    public bool? Branches { get; set; }

    [BooleanCommandSwitch("--tags")]
    public bool? Tags { get; set; }

    [BooleanCommandSwitch("--remotes")]
    public bool? Remotes { get; set; }

    [BooleanCommandSwitch("--glob")]
    public bool? Glob { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--exclude-hidden")]
    public bool? ExcludeHidden { get; set; }

    [CommandEqualsSeparatorSwitch("--disambiguate")]
    public string? Disambiguate { get; set; }

    [BooleanCommandSwitch("--local-env-vars")]
    public bool? LocalEnvVars { get; set; }

    [BooleanCommandSwitch("--path-format")]
    public bool? PathFormat { get; set; }

    [BooleanCommandSwitch("--git-dir")]
    public bool? GitDir { get; set; }

    [BooleanCommandSwitch("--git-common-dir")]
    public bool? GitCommonDir { get; set; }

    [CommandEqualsSeparatorSwitch("--resolve-git-dir")]
    public string? ResolveGitDir { get; set; }

    [CommandEqualsSeparatorSwitch("--git-path")]
    public string? GitPath { get; set; }

    [BooleanCommandSwitch("--show-toplevel")]
    public bool? ShowToplevel { get; set; }

    [BooleanCommandSwitch("--show-superproject-working-tree")]
    public bool? ShowSuperprojectWorkingTree { get; set; }

    [BooleanCommandSwitch("--shared-index-path")]
    public bool? SharedIndexPath { get; set; }

    [BooleanCommandSwitch("--absolute-git-dir")]
    public bool? AbsoluteGitDir { get; set; }

    [BooleanCommandSwitch("--is-inside-git-dir")]
    public bool? IsInsideGitDir { get; set; }

    [BooleanCommandSwitch("--is-inside-work-tree")]
    public bool? IsInsideWorkTree { get; set; }

    [BooleanCommandSwitch("--is-bare-repository")]
    public bool? IsBareRepository { get; set; }

    [BooleanCommandSwitch("--is-shallow-repository")]
    public bool? IsShallowRepository { get; set; }

    [BooleanCommandSwitch("--show-cdup")]
    public bool? ShowCdup { get; set; }

    [BooleanCommandSwitch("--show-prefix")]
    public bool? ShowPrefix { get; set; }

    [BooleanCommandSwitch("--show-object-format")]
    public bool? ShowObjectFormat { get; set; }

    [BooleanCommandSwitch("--since")]
    public bool? Since { get; set; }

    [BooleanCommandSwitch("--after")]
    public bool? After { get; set; }

    [BooleanCommandSwitch("--until")]
    public bool? Until { get; set; }

    [BooleanCommandSwitch("--before")]
    public bool? Before { get; set; }
}