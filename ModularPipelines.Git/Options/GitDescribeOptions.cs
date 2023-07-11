using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("describe")]
public record GitDescribeOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--dirty")]
    public string? Dirty { get; set; }

    [CommandEqualsSeparatorSwitch("--broken")]
    public string? Broken { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--tags")]
    public bool? Tags { get; set; }

    [BooleanCommandSwitch("--contains")]
    public bool? Contains { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [CommandEqualsSeparatorSwitch("--candidates")]
    public string? Candidates { get; set; }

    [BooleanCommandSwitch("--exact-match")]
    public bool? ExactMatch { get; set; }

    [BooleanCommandSwitch("--debug")]
    public bool? Debug { get; set; }

    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [CommandEqualsSeparatorSwitch("--match")]
    public string? Match { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--always")]
    public bool? Always { get; set; }

    [BooleanCommandSwitch("--first-parent")]
    public bool? FirstParent { get; set; }

}