using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("branch")]
[ExcludeFromCodeCoverage]
public record GitBranchOptions : GitOptions
{
    [BooleanCommandSwitch("--delete")]
    public virtual bool? Delete { get; set; }

    [BooleanCommandSwitch("--create-reflog")]
    public virtual bool? CreateReflog { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--move")]
    public virtual bool? Move { get; set; }

    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--copy")]
    public virtual bool? Copy { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public virtual bool? NoColor { get; set; }

    [BooleanCommandSwitch("--ignore-case")]
    public virtual bool? IgnoreCase { get; set; }

    [BooleanCommandSwitch("--omit-empty")]
    public virtual bool? OmitEmpty { get; set; }

    [CommandEqualsSeparatorSwitch("--column")]
    public string? Column { get; set; }

    [BooleanCommandSwitch("--no-column")]
    public virtual bool? NoColumn { get; set; }

    [BooleanCommandSwitch("--remotes")]
    public virtual bool? Remotes { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--list")]
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("--show-current")]
    public virtual bool? ShowCurrent { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--no-abbrev")]
    public virtual bool? NoAbbrev { get; set; }

    [BooleanCommandSwitch("--track")]
    public virtual bool? Track { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [CommandEqualsSeparatorSwitch("--set-upstream-to")]
    public string? SetUpstreamTo { get; set; }

    [BooleanCommandSwitch("--unset-upstream")]
    public virtual bool? UnsetUpstream { get; set; }

    [BooleanCommandSwitch("--edit-description")]
    public virtual bool? EditDescription { get; set; }

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