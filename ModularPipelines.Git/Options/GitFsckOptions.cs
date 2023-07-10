using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("fsck")]
public record GitFsckOptions : GitOptions
{
    [BooleanCommandSwitch("unreachable")]
    public bool? Unreachable { get; set; }

    [BooleanCommandSwitch("no-dangling")]
    public bool? NoDangling { get; set; }

    [BooleanCommandSwitch("dangling")]
    public bool? Dangling { get; set; }

    [BooleanCommandSwitch("root")]
    public bool? Root { get; set; }

    [BooleanCommandSwitch("tags")]
    public bool? Tags { get; set; }

    [BooleanCommandSwitch("cache")]
    public bool? Cache { get; set; }

    [BooleanCommandSwitch("no-reflogs")]
    public bool? NoReflogs { get; set; }

    [BooleanCommandSwitch("full")]
    public bool? Full { get; set; }

    [BooleanCommandSwitch("connectivity-only")]
    public bool? ConnectivityOnly { get; set; }

    [BooleanCommandSwitch("strict")]
    public bool? Strict { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("lost-found")]
    public bool? LostFound { get; set; }

    [BooleanCommandSwitch("name-objects")]
    public bool? NameObjects { get; set; }

    [BooleanCommandSwitch("no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("progress")]
    public bool? Progress { get; set; }

}