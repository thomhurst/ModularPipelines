using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("mv")]
public record GitMvOptions : GitOptions
{
    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

}