using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("branch")]
public record GitBranchOptions : GitOptions
{
    [BooleanCommandSwitch("--delete")]
    public bool? Delete { get; set; }

    [BooleanCommandSwitch("--create-reflog")]
    public bool? CreateReflog { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--move")]
    public bool? Move { get; set; }

    [BooleanCommandSwitch("--c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("--copy")]
    public bool? Copy { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public bool? NoColor { get; set; }

    [BooleanCommandSwitch("--ignore-case")]
    public bool? IgnoreCase { get; set; }

    [BooleanCommandSwitch("--omit-empty")]
    public bool? OmitEmpty { get; set; }

    [CommandEqualsSeparatorSwitch("--column")]
    public string? Column { get; set; }

    [BooleanCommandSwitch("--no-column")]
    public bool? NoColumn { get; set; }

    [BooleanCommandSwitch("--remotes")]
    public bool? Remotes { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("--show-current")]
    public bool? ShowCurrent { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--no-abbrev")]
    public bool? NoAbbrev { get; set; }

    [BooleanCommandSwitch("--track")]
    public bool? Track { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public bool? NoTrack { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--set-upstream")]
    public bool? SetUpstream { get; set; }

    [CommandEqualsSeparatorSwitch("--set-upstream-to")]
    public string? SetUpstreamTo { get; set; }

    [BooleanCommandSwitch("--unset-upstream")]
    public bool? UnsetUpstream { get; set; }

    [BooleanCommandSwitch("--edit-description")]
    public bool? EditDescription { get; set; }

    [CommandEqualsSeparatorSwitch("--contains")]
    public string? Contains { get; set; }

    [CommandEqualsSeparatorSwitch("--no-contains")]
    public string? NoContains { get; set; }

    [CommandEqualsSeparatorSwitch("--merged")]
    public string? Merged { get; set; }

    [CommandEqualsSeparatorSwitch("--no-merged")]
    public string? NoMerged { get; set; }

    [CommandEqualsSeparatorSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandEqualsSeparatorSwitch("--points-at")]
    public string? PointsAt { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }

}