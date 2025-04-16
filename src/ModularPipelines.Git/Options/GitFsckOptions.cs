using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("fsck")]
[ExcludeFromCodeCoverage]
public record GitFsckOptions : GitOptions
{
    [BooleanCommandSwitch("--unreachable")]
    public virtual bool? Unreachable { get; set; }

    [BooleanCommandSwitch("--no-dangling")]
    public virtual bool? NoDangling { get; set; }

    [BooleanCommandSwitch("--dangling")]
    public virtual bool? Dangling { get; set; }

    [BooleanCommandSwitch("--root")]
    public virtual bool? Root { get; set; }

    [BooleanCommandSwitch("--tags")]
    public virtual bool? Tags { get; set; }

    [BooleanCommandSwitch("--cache")]
    public virtual bool? Cache { get; set; }

    [BooleanCommandSwitch("--no-reflogs")]
    public virtual bool? NoReflogs { get; set; }

    [BooleanCommandSwitch("--full")]
    public virtual bool? Full { get; set; }

    [BooleanCommandSwitch("--connectivity-only")]
    public virtual bool? ConnectivityOnly { get; set; }

    [BooleanCommandSwitch("--strict")]
    public virtual bool? Strict { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--lost-found")]
    public virtual bool? LostFound { get; set; }

    [BooleanCommandSwitch("--name-objects")]
    public virtual bool? NameObjects { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }
}