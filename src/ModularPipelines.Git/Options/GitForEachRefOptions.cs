using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("for-each-ref")]
[ExcludeFromCodeCoverage]
public record GitForEachRefOptions : GitOptions
{
    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CommandEqualsSeparatorSwitch("--count")]
    public string? Count { get; set; }

    [CommandEqualsSeparatorSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--shell")]
    public virtual bool? Shell { get; set; }

    [BooleanCommandSwitch("--perl")]
    public virtual bool? Perl { get; set; }

    [BooleanCommandSwitch("--python")]
    public virtual bool? Python { get; set; }

    [BooleanCommandSwitch("--tcl")]
    public virtual bool? Tcl { get; set; }

    [CommandEqualsSeparatorSwitch("--points-at")]
    public string? PointsAt { get; set; }

    [CommandEqualsSeparatorSwitch("--merged")]
    public string? Merged { get; set; }

    [CommandEqualsSeparatorSwitch("--no-merged")]
    public string? NoMerged { get; set; }

    [CommandEqualsSeparatorSwitch("--contains")]
    public string? Contains { get; set; }

    [CommandEqualsSeparatorSwitch("--no-contains")]
    public string? NoContains { get; set; }

    [BooleanCommandSwitch("--ignore-case")]
    public virtual bool? IgnoreCase { get; set; }

    [BooleanCommandSwitch("--omit-empty")]
    public virtual bool? OmitEmpty { get; set; }
}