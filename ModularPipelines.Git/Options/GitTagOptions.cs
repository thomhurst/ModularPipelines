using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("tag")]
public record GitTagOptions : GitOptions
{
    [BooleanCommandSwitch("--annotate")]
    public bool? Annotate { get; set; }

    [BooleanCommandSwitch("--sign")]
    public bool? Sign { get; set; }

    [BooleanCommandSwitch("--no-sign")]
    public bool? NoSign { get; set; }

    [CommandEqualsSeparatorSwitch("--local-user")]
    public string? LocalUser { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--delete")]
    public bool? Delete { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [BooleanCommandSwitch("--list")]
    public bool? List { get; set; }

    [CommandEqualsSeparatorSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--ignore-case")]
    public bool? IgnoreCase { get; set; }

    [BooleanCommandSwitch("--omit-empty")]
    public bool? OmitEmpty { get; set; }

    [CommandEqualsSeparatorSwitch("--column")]
    public string? Column { get; set; }

    [BooleanCommandSwitch("--no-column")]
    public bool? NoColumn { get; set; }

    [CommandEqualsSeparatorSwitch("--contains")]
    public string? Contains { get; set; }

    [CommandEqualsSeparatorSwitch("--no-contains")]
    public string? NoContains { get; set; }

    [CommandEqualsSeparatorSwitch("--merged")]
    public string? Merged { get; set; }

    [CommandEqualsSeparatorSwitch("--no-merged")]
    public string? NoMerged { get; set; }

    [CommandEqualsSeparatorSwitch("--points-at")]
    public string? PointsAt { get; set; }

    [CommandEqualsSeparatorSwitch("--message")]
    public string? Message { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("--create-reflog")]
    public bool? CreateReflog { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }

}