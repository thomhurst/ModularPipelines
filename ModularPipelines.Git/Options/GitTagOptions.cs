using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("tag")]
public record GitTagOptions : GitOptions
{
    [BooleanCommandSwitch("annotate")]
    public bool? Annotate { get; set; }

    [BooleanCommandSwitch("sign")]
    public bool? Sign { get; set; }

    [BooleanCommandSwitch("no-sign")]
    public bool? NoSign { get; set; }

    [CommandLongSwitch("local-user")]
    public string? LocalUser { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("delete")]
    public bool? Delete { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

    [BooleanCommandSwitch("list")]
    public bool? List { get; set; }

    [CommandLongSwitch("sort")]
    public string? Sort { get; set; }

    [CommandLongSwitch("color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("ignore-case")]
    public bool? IgnoreCase { get; set; }

    [BooleanCommandSwitch("omit-empty")]
    public bool? OmitEmpty { get; set; }

    [CommandLongSwitch("column")]
    public string? Column { get; set; }

    [BooleanCommandSwitch("no-column")]
    public bool? NoColumn { get; set; }

    [CommandLongSwitch("contains")]
    public string? Contains { get; set; }

    [CommandLongSwitch("no-contains")]
    public string? NoContains { get; set; }

    [CommandLongSwitch("merged")]
    public string? Merged { get; set; }

    [CommandLongSwitch("no-merged")]
    public string? NoMerged { get; set; }

    [CommandLongSwitch("points-at")]
    public string? PointsAt { get; set; }

    [CommandLongSwitch("message")]
    public string? Message { get; set; }

    [CommandLongSwitch("file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("edit")]
    public bool? Edit { get; set; }

    [CommandLongSwitch("cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("create-reflog")]
    public bool? CreateReflog { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

}