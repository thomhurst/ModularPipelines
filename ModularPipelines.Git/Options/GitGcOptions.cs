using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("gc")]
public record GitGcOptions : GitOptions
{
    [BooleanCommandSwitch("--aggressive")]
    public bool? Aggressive { get; set; }

    [BooleanCommandSwitch("--auto")]
    public bool? Auto { get; set; }

    [BooleanCommandSwitch("--no-cruft")]
    public bool? NoCruft { get; set; }

    [BooleanCommandSwitch("--cruft")]
    public bool? Cruft { get; set; }

    [CommandEqualsSeparatorSwitch("--prune")]
    public string? Prune { get; set; }

    [BooleanCommandSwitch("--no-prune")]
    public bool? NoPrune { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--keep-largest-pack")]
    public bool? KeepLargestPack { get; set; }
}