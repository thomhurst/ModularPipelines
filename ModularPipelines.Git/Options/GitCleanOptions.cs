using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("clean")]
public record GitCleanOptions : GitOptions
{
    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [CommandLongSwitch("exclude")]
    public string? Exclude { get; set; }

}