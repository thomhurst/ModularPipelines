using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("grep")]
public record GitGrepOptions : GitOptions
{
    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("--no-index")]
    public bool? NoIndex { get; set; }

    [BooleanCommandSwitch("--untracked")]
    public bool? Untracked { get; set; }

    [BooleanCommandSwitch("--no-exclude-standard")]
    public bool? NoExcludeStandard { get; set; }

    [BooleanCommandSwitch("--exclude-standard")]
    public bool? ExcludeStandard { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--text")]
    public bool? Text { get; set; }

    [BooleanCommandSwitch("--textconv")]
    public bool? Textconv { get; set; }

    [BooleanCommandSwitch("--no-textconv")]
    public bool? NoTextconv { get; set; }

    [BooleanCommandSwitch("--ignore-case")]
    public bool? IgnoreCase { get; set; }

    [CommandEqualsSeparatorSwitch("--max-depth")]
    public string? MaxDepth { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--no-recursive")]
    public bool? NoRecursive { get; set; }

    [BooleanCommandSwitch("--word-regexp")]
    public bool? WordRegexp { get; set; }

    [BooleanCommandSwitch("--invert-match")]
    public bool? InvertMatch { get; set; }

    [BooleanCommandSwitch("--full-name")]
    public bool? FullName { get; set; }

    [BooleanCommandSwitch("--extended-regexp")]
    public bool? ExtendedRegexp { get; set; }

    [BooleanCommandSwitch("--basic-regexp")]
    public bool? BasicRegexp { get; set; }

    [BooleanCommandSwitch("--perl-regexp")]
    public bool? PerlRegexp { get; set; }

    [BooleanCommandSwitch("--fixed-strings")]
    public bool? FixedStrings { get; set; }

    [BooleanCommandSwitch("--line-number")]
    public bool? LineNumber { get; set; }

    [BooleanCommandSwitch("--column")]
    public bool? Column { get; set; }

    [BooleanCommandSwitch("--files-with-matches")]
    public bool? FilesWithMatches { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--files-without-match")]
    public bool? FilesWithoutMatch { get; set; }

    [CommandEqualsSeparatorSwitch("--open-files-in-pager")]
    public string? OpenFilesInPager { get; set; }

    [BooleanCommandSwitch("--null")]
    public bool? Null { get; set; }

    [BooleanCommandSwitch("--only-matching")]
    public bool? OnlyMatching { get; set; }

    [BooleanCommandSwitch("--c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("--count")]
    public bool? Count { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public bool? NoColor { get; set; }

    [BooleanCommandSwitch("--break")]
    public bool? Break { get; set; }

    [BooleanCommandSwitch("--heading")]
    public bool? Heading { get; set; }

    [BooleanCommandSwitch("--show-function")]
    public bool? ShowFunction { get; set; }

    [CommandEqualsSeparatorSwitch("--context")]
    public string? Context { get; set; }

    [CommandEqualsSeparatorSwitch("--after-context")]
    public string? AfterContext { get; set; }

    [CommandEqualsSeparatorSwitch("--before-context")]
    public string? BeforeContext { get; set; }

    [BooleanCommandSwitch("--function-context")]
    public bool? FunctionContext { get; set; }

    [CommandEqualsSeparatorSwitch("--max-count")]
    public string? MaxCount { get; set; }

    [CommandEqualsSeparatorSwitch("--threads")]
    public string? Threads { get; set; }

    [BooleanCommandSwitch("--and")]
    public bool? And { get; set; }

    [BooleanCommandSwitch("--or")]
    public bool? Or { get; set; }

    [BooleanCommandSwitch("--not")]
    public bool? Not { get; set; }

    [BooleanCommandSwitch("--all-match")]
    public bool? AllMatch { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}