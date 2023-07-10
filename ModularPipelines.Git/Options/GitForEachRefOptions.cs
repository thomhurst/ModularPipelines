using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("for-each-ref")]
public record GitForEachRefOptions : GitOptions
{
    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

    [CommandLongSwitch("count")]
    public string? Count { get; set; }

    [CommandLongSwitch("sort")]
    public string? Sort { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("shell")]
    public bool? Shell { get; set; }

    [BooleanCommandSwitch("perl")]
    public bool? Perl { get; set; }

    [BooleanCommandSwitch("python")]
    public bool? Python { get; set; }

    [BooleanCommandSwitch("tcl")]
    public bool? Tcl { get; set; }

    [CommandLongSwitch("points-at")]
    public string? PointsAt { get; set; }

    [CommandLongSwitch("merged")]
    public string? Merged { get; set; }

    [CommandLongSwitch("no-merged")]
    public string? NoMerged { get; set; }

    [CommandLongSwitch("contains")]
    public string? Contains { get; set; }

    [CommandLongSwitch("no-contains")]
    public string? NoContains { get; set; }

    [BooleanCommandSwitch("ignore-case")]
    public bool? IgnoreCase { get; set; }

    [BooleanCommandSwitch("omit-empty")]
    public bool? OmitEmpty { get; set; }

}