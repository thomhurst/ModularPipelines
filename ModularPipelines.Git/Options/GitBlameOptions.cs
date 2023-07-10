using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("blame")]
public record GitBlameOptions : GitOptions
{
    [BooleanCommandSwitch("root")]
    public bool? Root { get; set; }

    [BooleanCommandSwitch("show-stats")]
    public bool? ShowStats { get; set; }

    [CommandLongSwitch("reverse")]
    public string? Reverse { get; set; }

    [BooleanCommandSwitch("first-parent")]
    public bool? FirstParent { get; set; }

    [BooleanCommandSwitch("porcelain")]
    public bool? Porcelain { get; set; }

    [BooleanCommandSwitch("line-porcelain")]
    public bool? LinePorcelain { get; set; }

    [BooleanCommandSwitch("incremental")]
    public bool? Incremental { get; set; }

    [CommandLongSwitch("encoding")]
    public string? Encoding { get; set; }

    [CommandLongSwitch("contents")]
    public string? Contents { get; set; }

    [CommandLongSwitch("date")]
    public string? Date { get; set; }

    [BooleanCommandSwitch("no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("progress")]
    public bool? Progress { get; set; }

    [CommandLongSwitch("ignore-rev")]
    public string? IgnoreRev { get; set; }

    [CommandLongSwitch("ignore-revs-file")]
    public string? IgnoreRevsFile { get; set; }

    [BooleanCommandSwitch("color-lines")]
    public bool? ColorLines { get; set; }

    [BooleanCommandSwitch("color-by-age")]
    public bool? ColorByAge { get; set; }

    [BooleanCommandSwitch("c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("score-debug")]
    public bool? ScoreDebug { get; set; }

    [BooleanCommandSwitch("show-name")]
    public bool? ShowName { get; set; }

    [BooleanCommandSwitch("show-number")]
    public bool? ShowNumber { get; set; }

    [BooleanCommandSwitch("show-email")]
    public bool? ShowEmail { get; set; }

    [CommandLongSwitch("abbrev")]
    public string? Abbrev { get; set; }

}