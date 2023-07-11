using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("remote")]
public record GitRemoteOptions : GitOptions
{
    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }
}