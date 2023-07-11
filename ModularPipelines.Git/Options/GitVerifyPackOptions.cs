using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("verify-pack")]
public record GitVerifyPackOptions : GitOptions
{
    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--stat-only")]
    public bool? StatOnly { get; set; }

}